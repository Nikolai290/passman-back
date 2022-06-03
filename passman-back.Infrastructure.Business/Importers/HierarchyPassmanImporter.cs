using AutoMapper;
using passman_back.Business.Dtos.ImportPassmanHierarchy;
using passman_back.Business.Interfaces.Importers;
using passman_back.Domain.Core.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace passman_back.Infrastructure.Business.Importers {
    public class HierarchyPassmanImporter : IHierarchyPassmanImporter {
        private readonly IMapper mapper;

        public HierarchyPassmanImporter(IMapper mapper) {
            this.mapper = mapper;
        }

        public IList<Passcard> GetPascards(
            PassmanHierarchyImportDto importDto,
            IList<Domain.Core.DbEntities.Directory> directories
        ) {
            var passcards = mapper.Map<IList<Passcard>>(importDto.Passcards);

            for (var i = 0; i < importDto.Passcards.Count; i++) {
                var passImportDto = importDto.Passcards[i];
                var passcard = passcards[i];
                var parentIds = passImportDto.ParentIds;
                passcard.Parents = new List<passman_back.Domain.Core.DbEntities.Directory>();

                for (var j = 0; j < importDto.Directories.Count; j++) {
                    var dirImporDto = importDto.Directories[j];
                    if (!parentIds.Contains(dirImporDto.Id)) {
                        continue;
                    }
                    var directory = directories[j];
                    passcard.Parents.Add(directory);
                }
            }

            return passcards;
        }

        public IList<Domain.Core.DbEntities.Directory> GetDirectories(
            IList<DirectoryImportDto> importDirectoryDtos
        ) {
            var directories = mapper.Map<IList<passman_back.Domain.Core.DbEntities.Directory>>(importDirectoryDtos);

            for (var i = 0; i < directories.Count; i++) {
                var dirImporDto = importDirectoryDtos[i];
                if (dirImporDto.ParentId == 0) {
                    continue;
                }

                var dir = directories[i];

                for (var j = 0; j < directories.Count; j++) {
                    var parentImportDto = importDirectoryDtos[j];
                    if (parentImportDto.Id != dirImporDto.ParentId) {
                        continue;
                    }

                    var parent = directories[j];
                    dir.Parent = parent;
                    break;
                }
            }

            return directories;
        }
    }
}