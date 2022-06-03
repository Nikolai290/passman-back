using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using passman_back.Business.Interfaces.Exporters;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.Repositories;
using passman_back.Infrastructure.Business.Enums;
using System;
using System.IO;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Services {
    public class ExportService : BaseService, IExportService {

        private readonly IPasscardRepository passcardRepository;
        private readonly IDirectoryRepository directoryRepository;
        private readonly IMapper mapper;
        private readonly ICrypter crypter;
        private readonly IFlatExporter fLatExporter;
        private readonly IHierarchyPassmanExporter hierarchyPassmanExporter;

        public ExportService(
            IPasscardRepository passcardRepository,
            IDirectoryRepository directoryRepository,
            ICrypter crypter,
            IMapper mapper,
            IFlatExporter fLatExporter,
            IHierarchyPassmanExporter hierarchyPassmanExporter,
            IUserRepository userRepository,
            IHttpContextAccessor accessor,
            IValidator<User> userValidator
        ) : base(userRepository, accessor, userValidator) {
            this.passcardRepository = passcardRepository;
            this.directoryRepository = directoryRepository;
            this.crypter = crypter;
            this.mapper = mapper;
            this.fLatExporter = fLatExporter;
            this.hierarchyPassmanExporter = hierarchyPassmanExporter;
        }

        public async Task ExportAsync(string type, MemoryStream stream) {
            var user = await GetCurrentUserAndValidateAsync();
            if (!user.IsAdmin) {
                throw new UnauthorizedAccessException();
            }

            var passcards = await passcardRepository.GetAllAsync();
            string[] headers = { "name", "url", "login", "password", "description" };

            switch (type) {
                case Export.Csv:
                    fLatExporter.ExportToCsv(passcards, stream, headers);
                    return;
                case Export.Xlsx:
                    fLatExporter.ExportToXlsx(passcards, stream, headers);
                    return;
                case Export.Xls:
                    fLatExporter.ExportToXlsx(passcards, stream, headers);
                    return;
                case Export.Json:
                    return;
                default:
                    throw new Exception("Экспорт в выбранном расширени невозможен");
            }
        }

        public async Task ExportHierarchyPassmanAsync(MemoryStream stream, bool encrypted) {
            var user = await GetCurrentUserAndValidateAsync();
            if (!user.IsAdmin) {
                throw new UnauthorizedAccessException();
            }

            var directories = await directoryRepository.GetAllAsync();
            var passcards = await passcardRepository.GetAllAsync();

            hierarchyPassmanExporter.ExportHierarchyPassmanJson(stream, encrypted, directories, passcards);
        }
    }
}
