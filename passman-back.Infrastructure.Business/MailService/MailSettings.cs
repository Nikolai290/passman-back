using System;
namespace passman_back.Infrastructure.Business.MailService {
    public class MailSettings {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isFull
            => !string.IsNullOrEmpty(Host)
            && !string.IsNullOrEmpty(Email)
            && !string.IsNullOrEmpty(Password)
            && Port > 0 && Port <= 65535;

        public MailSettings GetSettings() {
            var settings = new MailSettings() {
                Host = Environment.GetEnvironmentVariable("EMAIL_HOST"),
                Port = TryGetPort(),
                Email = Environment.GetEnvironmentVariable("EMAIL_EMAIL"),
                Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD")
            };

            return settings.isFull ? settings : this;
        }

        private int TryGetPort() {
            try {
                var port = Environment.GetEnvironmentVariable("EMAIL_PORT");
                return !string.IsNullOrEmpty(port) ? int.Parse(port) : 0;
            } catch {
                return 0;
            }
        }
    }
}
