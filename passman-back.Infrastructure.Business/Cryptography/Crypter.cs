using passman_back.Business.Interfaces.Services;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Cryptography {
    public class Crypter : ICrypter {
        private static string key;

        public Crypter() { }

        public static void SetKey(string key) {
            Crypter.key = key;
        }

        public static async Task SetKeyFromXmlByPath(string path) {
            try {
                key = await GetXmlKeyAsync(path);
            } catch (FileNotFoundException e) {
                key = await CreateNewXmlKeyAsync(path);
            }
        }

        public static async Task<string> GetXmlKeyAsync(string path) {
            string result;
            CreateDirectory(path);
            using (var stream = new StreamReader(path)) {
                result = await stream.ReadToEndAsync();
            }
            return result;
        }

        private static async Task<string> CreateNewXmlKeyAsync(string path) {
            var xmlKey = "";
            CreateDirectory(path);

            using (var stream = new StreamWriter(path)) {
                RSACryptoServiceProvider rsa = new();
                xmlKey = rsa.ToXmlString(true);
                await stream.WriteAsync(xmlKey);
            }

            return xmlKey;
        }

        private static void CreateDirectory(string path) {
            var file = path.Split('/').Last();
            var dir = path.Substring(0, path.Length - file.Length);
            var fullPath = Path.GetFullPath(dir);
            if (!Directory.Exists(fullPath)) {
                Directory.CreateDirectory(fullPath);
            }
        }

        public string Decrypt(string p) {
            byte[] decryptedContent;
            RSACryptoServiceProvider rsa = new();
            rsa.FromXmlString(key);
            decryptedContent = rsa.Decrypt(Convert.FromBase64String(p), false);

            return _toString(decryptedContent);
        }

        public string Encrypt(string p) {
            byte[] encryptedContent;

            RSACryptoServiceProvider rsa = new();
            rsa.FromXmlString(key);
            encryptedContent = rsa.Encrypt(_toByte(p), false);

            return Convert.ToBase64String(encryptedContent);
        }

        private static string _toString(byte[] decryptedContent) {
            return Encoding.UTF8.GetString(decryptedContent);
        }

        private static byte[] _toByte(string p) {
            return Encoding.UTF8.GetBytes(p);
        }
    }
}
