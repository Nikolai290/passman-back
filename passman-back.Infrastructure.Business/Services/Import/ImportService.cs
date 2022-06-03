using FluentValidation;
using Microsoft.AspNetCore.Http;
using passman_back.Business.Dtos.ImportBitwardenHierarchy;
using passman_back.Business.Dtos.ImportPassmanHierarchy;
using passman_back.Business.Interfaces.Importers;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.Repositories;
using passman_back.Infrastructure.Business.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Services {
    public class ImportService : BaseService, IImportService {
        private readonly IDirectoryRepository directoryRepository;
        private readonly IPasscardRepository passcardRepository;
        private readonly IFlatImporter flatImporter;
        private readonly IHierarchyBitwardernImporter hierarchyBitwardernImporter;
        private readonly IHierarchyPassmanImporter hierarchyPassmanImporter;
        private readonly ICrypter crypter;

        public ImportService(
            IPasscardRepository passcardRepository,
            IDirectoryRepository directoryRepository,
            IUserRepository userRepository,
            IHttpContextAccessor accessor,
            IValidator<User> userValidator,
            ICrypter crypter,
            IFlatImporter flatImporter,
            IHierarchyPassmanImporter hierarchyPassmanImporter,
            IHierarchyBitwardernImporter hierarchyBitwardernImporter
        ) : base(userRepository, accessor, userValidator) {
            this.passcardRepository = passcardRepository;
            this.directoryRepository = directoryRepository;
            this.crypter = crypter;
            this.flatImporter = flatImporter;
            this.hierarchyPassmanImporter = hierarchyPassmanImporter;
            this.hierarchyBitwardernImporter = hierarchyBitwardernImporter;
        }

        public async Task ImportAsync(IFormFile file) {
            var directory = new Domain.Core.DbEntities.Directory() {
                Name = file.FileName + "__" + DateTime.Now.ToString("s"),
            };

            var fileType = file.FileName.Split('.').Last();

            var passcards = await SwitchByTypeAsync(fileType);

            await passcardRepository.CreateRangeAsync(passcards);

            Task<IEnumerable<Passcard>> SwitchByTypeAsync(string fileType) {
                switch (fileType) {
                    case Export.Csv:
                        return flatImporter.ImportFromCsv(file, directory);
                    case Export.Xls:
                        return flatImporter.ImportFromXlsx(file, directory);
                    case Export.Xlsx:
                        return flatImporter.ImportFromXlsx(file, directory);
                    default:
                        throw new Exception("Неожиданный формат");
                }
            }
        }

        public async Task ImportHierarchyPassmanAsync(IFormFile file) {
            var user = await GetCurrentUserAndValidateAsync();
            if (!user.IsAdmin) {
                throw new UnauthorizedAccessException();
            }

            if (file.Length == 0) {
                throw new Exception("file length is 0");
            }

            var importDto = await GetImportDtoAsync<PassmanHierarchyImportDto>(file);

            if (importDto == null) {
                throw new Exception("import dto is null");
            }

            var directories = hierarchyPassmanImporter.GetDirectories(importDto.Directories);
            await directoryRepository.CreateRangeAsync(directories);

            var passcards = hierarchyPassmanImporter.GetPascards(importDto, directories);
            await passcardRepository.CreateRangeAsync(passcards);
        }

        public async Task ImportHierarchyBitwardenAsync(IFormFile file) {
            var user = await GetCurrentUserAndValidateAsync();
            if (!user.IsAdmin) {
                throw new UnauthorizedAccessException();
            }

            if (file.Length == 0) {
                throw new Exception("file length is 0");
            }

            var importDto = await GetImportDtoAsync<BitwardenHierarchyDto>(file);

            if (importDto == null) {
                throw new Exception("import dto is null");
            }

            var directories = hierarchyBitwardernImporter.GetDirectories(importDto.Folders);
            await directoryRepository.CreateRangeAsync(directories);

            var passcards = hierarchyBitwardernImporter.GetPascards(importDto, directories);
            await passcardRepository.CreateRangeAsync(passcards);
        }

        private async Task<T> GetImportDtoAsync<T>(IFormFile file) {
            string importData = default;
            using (var stream = new MemoryStream()) {
                await file.CopyToAsync(stream);
                importData = Encoding.UTF8.GetString(stream.ToArray());
                stream.Close();
            }
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };
            var importDto = JsonSerializer.Deserialize<T>(importData, options);

            return importDto;
        }
    }
}
