using AutoMapper;
using passman_back.Business.Dtos.ImportBitwardenHierarchy;
using passman_back.Business.Interfaces.Importers;
using passman_back.Domain.Core.DbEntities;
using System;
using System.Collections.Generic;

namespace passman_back.Infrastructure.Business.Importers {
    public class HierarchyBitwardernImporter : IHierarchyBitwardernImporter {

        private readonly IMapper mapper;

        public HierarchyBitwardernImporter(IMapper mapper) {
            this.mapper = mapper;
        }

        public IList<Passcard> GetPascards(BitwardenHierarchyDto importDto, IList<Domain.Core.DbEntities.Directory> directories) {
            var root = new Directory() {
                Name = "bitwarden_root_import__" + DateTime.Now.ToString(),
                Passcards = new List<Passcard>()
            };
            IList<PasscardImportBitwardenDto> passcardDtos = MappingPasscardDtos(importDto);
            IList<Passcard> passcards = mapper.Map<IList<Passcard>>(passcardDtos);

            for (var i = 0; i < passcards.Count; i++) {
                var passcard = passcards[i];
                var item = passcardDtos[i];
                passcard.Parents = new List<Directory>();

                if (item.FolderId is null) {
                    passcard.Parents.Add(root);
                    continue;
                }

                for (var j = 0; j < passcards.Count; j++) {
                    var directory = directories[j];
                    var folder = importDto.Folders[j];
                    if (folder.Id == item.FolderId) {
                        passcard.Parents.Add(directory);
                        break;
                    }
                }
            }
            return passcards;
        }

        IList<Directory> IHierarchyBitwardernImporter.GetDirectories(IList<Folder> folders) {
            var directories = mapper.Map<IList<Directory>>(folders);
            return directories;
        }

        private IList<PasscardImportBitwardenDto> MappingPasscardDtos(BitwardenHierarchyDto importDto) {
            IList<PasscardImportBitwardenDto> passcardDtos = new List<PasscardImportBitwardenDto>();
            foreach (var item in importDto.Items) {
                for (var i = 0; i < item.Login.Uris.Count; i++) {
                    var uri = item.Login.Uris[i].Uri;
                    var passcardDto = mapper.Map<PasscardImportBitwardenDto>(item);
                    passcardDto.Url = uri;
                    passcardDtos.Add(passcardDto);
                }
            }

            return passcardDtos;
        }
    }
}