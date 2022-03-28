namespace passman_back.Domain.Core.DbEntities {
    public abstract class AbstractDbEntity {
        public long Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
