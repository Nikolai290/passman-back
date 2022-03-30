using Microsoft.Extensions.DependencyInjection;
using passman_back.Business.Interfaces.Services;
using passman_back.Infrastructure.Business.Services;

namespace passman_back.IoC {
    public static class ServiceModule {
        public static void ConfigureServices(this IServiceCollection services) {
            services.AddScoped<IDirectoryService, DirectoryService>();
            services.AddScoped<IPasscardService, PasscardService>();
        }
    }
}
