using AutoMapper;
using passman_back.Business.Dtos.ExportPassmanHierarchy;
using passman_back.Business.Interfaces.Exporters;
using passman_back.Domain.Core.DbEntities;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;


namespace passman_back.Infrastructure.Business.Exporters {
    public class HierarchyPassmanExporter : IHierarchyPassmanExporter {
        private readonly IMapper mapper;

        public HierarchyPassmanExporter(IMapper mapper) {
            this.mapper = mapper;
        }

        public void ExportHierarchyPassmanJson(MemoryStream stream, bool encrypted, IList<Domain.Core.DbEntities.Directory> directories, IList<Passcard> passcards) {
            var exportDto = new PassmanHierarchyExportDto() {
                Encrypted = encrypted
            };

            exportDto.Directories = mapper.Map<IList<DirectoryExportDto>>(directories);
            exportDto.Passcards = encrypted
                ? mapper.Map<IList<PasscardExportDto>>(
                        mapper.Map<IList<PasscardExportCryptedDto>>(passcards)
                    )
                : mapper.Map<IList<PasscardExportDto>>(passcards);

            var options = new JsonSerializerOptions() {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };

            var exportData = JsonSerializer.Serialize(exportDto, options);

            using (var writer = new StreamWriter(stream, leaveOpen: true)) {
                writer.WriteLine(exportData);
                writer.Flush();
                stream.Position = 0;
            }
        }
    }
}