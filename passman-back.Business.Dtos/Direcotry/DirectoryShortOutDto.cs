using passman_back.Domain.Core.Enums;

namespace passman_back.Business.Dtos {
    public class DirectoryShortOutDto {
        public long Id { get; set; }
        public string Name { get; set; }
        public Permission Permission { get; set; }
    }
}
