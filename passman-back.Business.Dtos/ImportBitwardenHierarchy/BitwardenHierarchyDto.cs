using System.Collections.Generic;

namespace passman_back.Business.Dtos.ImportBitwardenHierarchy {
    public class BitwardenHierarchyDto {
        public bool Encrypted { get; set; }
        public IList<Folder> Folders { get; set; }
        public IList<Item> Items { get; set; }
    }
}