using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.DbContexts;
using passman_back.Infrastructure.Domain.Settings;
using System.Reflection;

namespace passman_back.Infrastructure.Data.DbContexts {
    public partial class MainDbContext : DbContext, IMainDbContext {
        public DbSet<Directory> Directories { get; set; }
        public DbSet<Passcard> Passcards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserGroupDirectoryRelation> UserGroupDirectoryRelations { get; set; }
        public DbSet<RestorePasswordCode> RestorePasswordCodes { get; set; }
        public DbSet<InviteCode> InviteCodes { get; set; }

        public MainDbSettings settings { get; set; }

        public MainDbContext() : base() { }

        public MainDbContext(
            DbContextOptions<MainDbContext> options,
            IOptions<MainDbSettings> settings
        ) : base(options) {
            this.settings = settings.Value;
            Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder
                .UseSnakeCaseNamingConvention()
                .UseLazyLoadingProxies()
                .UseNpgsql(settings.GetConnectionsString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}