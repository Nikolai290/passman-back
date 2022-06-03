using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using passman_back.Infrastructure.Business.Cryptography;
using System.IO;
using System.Linq;

namespace passman_back {
    public class Program {
        private static string RsaKeyPath = "RSA/key.xml";
        public static void Main(string[] args) {
            ReadArgs(args);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((hostingContext, builder) => {
                    var env = hostingContext.HostingEnvironment.EnvironmentName;
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile("appsettings.json");
                    builder.AddJsonFile($"appsettings.{env}.json");
                });
        }

        public static void ReadArgs(string[] args) {
            RsaKeyPath = GetParametr("xmlKeyPath", args) ?? RsaKeyPath;
            Crypter.SetKeyFromXmlByPath(RsaKeyPath).GetAwaiter();
        }

        public static string GetParametr(string parametr, string[] args) {
            return args.FirstOrDefault(x => x.Contains(parametr))?.Split('=').Last();
        }
    }
}
