using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using passman_back.Business.Dtos;
using passman_back.Domain.Core.DbEntities;
using passman_back.Infrastructure.Business.Validators;
using passman_back.Infrastructure.Business.Validators.User;
using passman_back.Infrastructure.Business.Validators.UserGroupDirectoryRelation;

namespace passman_back.IoC {
    public static class ValidatorModule {
        public static void ConfigureValidators(this IServiceCollection services) {
            // Directory
            services.AddTransient<IValidator<DirectoryCreateDto>, DirectoryCreateDtoValidator>();
            services.AddTransient<IValidator<DirectoryUpdateDto>, DirectoryUpdateDtoValidator>();
            // Passcard
            services.AddTransient<IValidator<PasscardCreateDto>, PasscardCreateDtoValidator>();
            services.AddTransient<IValidator<PasscardUpdateDto>, PasscardUpdateDtoValidator>();
            //User
            services.AddTransient<IValidator<UserAdminCreateDto>, UserAdminCreateDtoValidator>();
            services.AddTransient<IValidator<UserRegisterDto>, RegisterDtoValidator>();
            services.AddTransient<IValidator<UserUpdateDto>, UpdateUserDtoValidator>();
            services.AddTransient<IValidator<UserChangePasswordDto>, ChangePasswordDtoValidator>();
            services.AddTransient<IValidator<UserRestorePasswordStepTwoDto>, UserRestorePasswordDtoValidator>();
            services.AddTransient<IValidator<User>, UserValidator>();
            // Admin user
            services.AddTransient<IValidator<UserAdminUpdateDto>, UserAdminUpdateDtoValidator>();

            // User groups
            services.AddTransient<IValidator<UserGroupCreateDto>, UserGroupCreateDtoValidator>();
            services.AddTransient<IValidator<UserGroupUpdateDto>, UserGroupUpdateDtoValidator>();

            // User group - directory relations
            services.AddTransient<IValidator<UserGroupDirectoryRelationCreateDto>, UserGroupDirectoryRelationCreateDtoValidator>();
            services.AddTransient<IValidator<UserGroupDirectoryRelationUpdateDto>, UserGroupDirectoryRelationUpdateDtoValidator>();

            // Invite codes
            services.AddTransient<IValidator<InviteCodeCreateDto>, InviteCodeCreateDtoValidator>();
            services.AddTransient<IValidator<InviteCodeUpdateDto>, InviteCodeUpdateDtoValidator>();
        }
    }
}
