using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using passman_back.Business.Interfaces.Importers;
using passman_back.Business.Interfaces.Services;
using passman_back.Domain.Core.DbEntities;
using passman_back.Infrastructure.Business.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Importers {
    public class FlatImporter : IFlatImporter {
        private static readonly string[] neccessary = { "name", "url", "login", "user", "username", "password", "description" };
        private readonly ICrypter crypter;

        public FlatImporter(ICrypter crypter) {
            this.crypter = crypter;
        }
        public async Task<IEnumerable<Passcard>> ImportFromCsv(IFormFile file, Domain.Core.DbEntities.Directory directory) {
            var passcards = new List<Passcard>();

            if (file.Length > 0) {
                using (var stream = new MemoryStream()) {
                    await file.CopyToAsync(stream);
                    string dataString = Encoding.UTF8.GetString(stream.ToArray());
                    var data = new Queue<string>(dataString.Replace("\r\n", "\n").Split("\n"));

                    var headers = data.Dequeue().ToLower().Split(',');
                    CheckHeaders(headers);

                    while (data.Count > 0) {
                        var line = data.Dequeue().Split(',');
                        var passcard = ParsePasscard(headers, line);
                        passcard.Parents = new List<Domain.Core.DbEntities.Directory>() { directory };
                        passcards.Add(passcard);
                    }
                    stream.Close();
                }
            }

            return passcards;
        }
        public async Task<IEnumerable<Passcard>> ImportFromXlsx(IFormFile file, Domain.Core.DbEntities.Directory directory) {
            var passcards = new List<Passcard>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var stream = new MemoryStream()) {

                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream)) {
                    var sheet = package.Workbook.Worksheets.First();
                    var headers = GetHeadersFromXlsx(sheet);
                    var lines = GetLinesFromXlsx(sheet, headers.Length);

                    foreach (var line in lines) {
                        var passcard = ParsePasscard(headers, line.Split(','));
                        passcard.Parents = new List<Domain.Core.DbEntities.Directory>() { directory };
                        passcards.Add(passcard);
                    }
                }
            }

            return passcards;
        }

        private static string[] GetLinesFromXlsx(ExcelWorksheet sheet, int length) {
            var offset = 2;
            var lines = new List<string>();
            while (true) {
                var cells = sheet.Cells[offset, 1, offset, length];
                if (cells.All(x => string.IsNullOrEmpty(x.Value.ToString()))) {
                    break;
                }
                string line = "";
                for (var i = 0; i < length; i++) {
                    var cell = sheet.Cells[offset, i + 1];
                    line += cell.Value == null ? "," : cell.Value.ToString() + ",";
                }
                lines.Add(line);
                offset++;
            }

            return lines.ToArray();
        }

        private static string[] GetHeadersFromXlsx(ExcelWorksheet sheet) {
            var headers = new List<string>();
            var cells = sheet.Cells[1, 1, 1, 20];
            foreach (var cell in cells) {
                if (string.IsNullOrEmpty(cell.Value.ToString())) {
                    break;
                }
                headers.Add(cell.Value.ToString());
            }
            CheckHeaders(headers);
            return headers.ToArray();
        }

        private static void CheckHeaders(IEnumerable<string> headers) {
            if (headers.Intersect(neccessary).Count() == 0) {
                throw new NoHeaderException("Заголовок не обнаружен. Импорт файла невозможен.");
            }
        }

        private Passcard ParsePasscard(string[] header, string[] line) {
            var passcard = new Passcard();
            for (var i = 0; i < header.Length; i++) {
                if (line.Length - 1 < i) {
                    continue;
                }
                var field = header[i];
                var value = line[i];
                switch (field) {
                    case "name":
                        passcard.Name = value;
                        break;
                    case "url":
                        passcard.Url = value;
                        break;
                    case "username":
                        passcard.Login = value;
                        break;
                    case "user":
                        passcard.Login = value;
                        break;
                    case "login":
                        passcard.Login = value;
                        break;
                    case "password":
                        passcard.Password = TryEncrypt(value);
                        break;
                    case "description":
                        passcard.Description = value;
                        break;
                }
            }
            return passcard;
        }

        private string TryEncrypt(string password) {
            try {
                return crypter.Encrypt(password);
            } catch {
                return password;
            }
        }
    }
}