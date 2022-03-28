using System.Collections.Generic;

namespace passman_back.Business.Dtos {
    public class DirectoryOutDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public IList<DirectoryOutDto> Childrens { get; set; }
        public IList<PasscardOutDto> Passcards { get; set; }
    }
}
