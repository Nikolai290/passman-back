using Microsoft.Extensions.DependencyInjection;
using passman_back.Business.Interfaces.Services;
using passman_back.Infrastructure.Business.Cryptography;
using passman_back.Infrastructure.Business.MailService;
using passman_back.Infrastructure.Business.Services;
using SearchingLibrary.Service;
using SearchingLibrary.ServiceInterface;

namespace passman_back.IoC {
    public static class ServiceModule {
        public static void ConfigureServices(this IServiceCollection services) {
            services.AddScoped<IDirectoryService, DirectoryService>();
            services.AddScoped<IPasscardService, PasscardService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICrypter, Crypter>();
            services.AddScoped<IMailer, Mailer>();
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<IUserGroupDirectoryRelationService, UserGroupDirectoryRelationService>();
            services.AddScoped<IAdminUserGroupsService, AdminUserGroupsService>();
            services.AddScoped<ISearcher, Searcher>();
            services.AddScoped<IInviteCodeService, InviteCodeService>();
            services.AddScoped<IExportService, ExportService>();
            services.AddScoped<IImportService, ImportService>();
            services.AddScoped<IFavoritePasscardService, FavoritePasscardService>();
        }
    }
}
