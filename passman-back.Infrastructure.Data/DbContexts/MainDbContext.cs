using Microsoft.EntityFrameworkCore;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.DbContexts;
using System.Reflection;

namespace passman_back.Infrastructure.Data.DbContexts {
    public partial class MainDbContext : DbContext, IMainDbContext {
        public DbSet<Directory> Directories { get; set; }
        public DbSet<Passcard> Passcards { get; set; }

        public MainDbContext() : base() { }

        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            var connetctionString = "Host=10.214.1.247;Port=5433;Database=passman;Username=passman;Password=passman551";

            optionsBuilder
                .UseSnakeCaseNamingConvention()
                .UseLazyLoadingProxies()
                .UseNpgsql(connetctionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}