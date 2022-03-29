namespace passman_back.Domain.Core.DbEntities {
    public abstract class AbstractDbEntity {
        public virtual long Id { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}
