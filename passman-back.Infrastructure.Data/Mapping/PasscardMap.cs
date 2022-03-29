using Microsoft.EntityFrameworkCore.Metadata.Builders;
using passman_back.Domain.Core.DbEntities;

namespace passman_back.Infrastructure.Data.Mapping {
    public class PasscardMap : BaseMap<Passcard>{

        public PasscardMap() { }

        public override void Configure(EntityTypeBuilder<Passcard> builder) {
            base.Configure(builder);
        }
    }
}
