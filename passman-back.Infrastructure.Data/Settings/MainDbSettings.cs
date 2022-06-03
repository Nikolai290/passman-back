using System;

namespace passman_back.Infrastructure.Domain.Settings {
    public class MainDbSettings {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 5433;
        public string Database { get; set; } = "passman";
        public string Password { get; set; } = "passman";
        public string Username { get; set; } = "passman551";

        public string GetConnectionsString() {
            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? Host;
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? Port.ToString();
            var database = Environment.GetEnvironmentVariable("ASPNETCORE_DB_NAME") ?? Database;
            var username = Environment.GetEnvironmentVariable("DB_USER") ?? Username;
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? Password;

            return
                $"Host={host};" +
                $"Port={port};" +
                $"Database={database};" +
                $"Username={username};" +
                $"Password={password}";
        }
    }
}
