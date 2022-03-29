using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using passman_back.Domain.Core.DbEntities;

namespace passman_back.Infrastructure.Data.Mapping {
    public class DirectoryMap : BaseMap<Directory>{

        public DirectoryMap() { }
        public override void Configure(EntityTypeBuilder<Directory> builder) {
            base.Configure(builder);

            builder
                .HasMany(x => x.Passcards)
                .WithMany(x => x.Parents)
                .UsingEntity(x => x.ToTable("directory_passcards_relations"));
        }
    }
}
