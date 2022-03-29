using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using passman_back.Domain.Core.DbEntities;

namespace passman_back.Infrastructure.Data.Mapping {
    public class BaseMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : AbstractDbEntity {

        public BaseMap() { }
        public virtual void Configure(EntityTypeBuilder<TEntity> builder) {

            builder
                .HasKey(x => x.Id);
            builder
                .Property(x => x.IsDeleted)
                .HasColumnType("boolean")
                .HasDefaultValue(false);
        }
    }
}
