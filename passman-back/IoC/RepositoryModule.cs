using Microsoft.Extensions.DependencyInjection;
using passman_back.Domain.Interfaces.Repositories;
using passman_back.Infrastructure.Domain.Repositories;

namespace passman_back.IoC {
    public static class RepositoryModule {
        public static void ConfigureRepositoty(this IServiceCollection services) {
            services.AddScoped<IDirectoryRepository, DirectoryRepository>();
            services.AddScoped<IPasscardRepository, PasscardRepository>();
        }
    }
}
