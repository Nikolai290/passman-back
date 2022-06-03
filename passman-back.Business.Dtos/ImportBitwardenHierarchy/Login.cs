using System.Collections.Generic;

namespace passman_back.Business.Dtos.ImportBitwardenHierarchy {
    public class Login {
        public IList<Url> Uris { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}