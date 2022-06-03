using System.Collections.Generic;

namespace passman_back.Business.Dtos {
    public class PasscardOutDto {
        public long Id { get; set; }
        public IList<long> ParentIds { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public bool IsFavorite { get; set; }
    }
}