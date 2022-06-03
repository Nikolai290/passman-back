using System.Net;
using System;
using System.Linq;

namespace passman_back.Infrastructure.Business.Settigns {
    public class PassmanSettings {
        public int MaxLengthOfTextFields { get; set; }
        public int MinPasswordLengthForUsers { get; set; }
        public string FrontEndUrl { get; set; }
        public string GetFrontendUrl() {
            var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? FrontEndUrl;

            return frontendUrl;
        }
    }
}
