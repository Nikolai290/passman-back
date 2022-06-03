using OfficeOpenXml;
using passman_back.Business.Interfaces.Exporters;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace passman_back.Infrastructure.Business.Exporters {
    public class FlatExporter : IFlatExporter {

        private readonly ICrypter crypter;

        public FlatExporter(
            ICrypter crypter
        ) {
            this.crypter = crypter;
        }

        public void ExportToXlsx(IList<Passcard> passcards, MemoryStream stream, string[] headers) {
            var offset = 1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(stream)) {
                package.Workbook.Worksheets.Add("exportPasswords");
                var sheet = package.Workbook.Worksheets.First(x => x.Name == "exportPasswords");
                for (var cell = 0; cell < headers.Length; cell++) {
                    sheet.Cells[1, cell + 1].Value = headers[cell];
                }
                offset++;
                for (var row = 0; row < passcards.Count(); row++) {
                    var passcard = passcards[row];
                    sheet.Cells[row + offset, 1].Value = passcard.Name;
                    sheet.Cells[row + offset, 2].Value = passcard.Url;
                    sheet.Cells[row + offset, 3].Value = passcard.Login;
                    sheet.Cells[row + offset, 4].Value = TryDecrypt(passcard.Password);
                    sheet.Cells[row + offset, 5].Value = passcard.Description;
                }

                package.Save();
            }
            stream.Position = 0;
        }

        public void ExportToCsv(IEnumerable<Passcard> passcards, Stream stream, string[] headers) {
            using (var writer = new StreamWriter(stream, leaveOpen: true)) {
                writer.WriteLine(string.Join(',', headers));
                foreach (var passcard in passcards) {
                    writer.WriteLine(PasscardToString(passcard));
                }
                writer.Flush();
                stream.Position = 0;
            }
        }

        private string PasscardToString(Passcard passcard) {
            return string.Join(",",
                passcard.Name,
                passcard.Url,
                passcard.Login,
                TryDecrypt(passcard.Password),
                passcard.Description
            );
        }

        private string TryDecrypt(string password) {
            try {
                return crypter.Decrypt(password);
            } catch {
                return password;
            }
        }
    }
}