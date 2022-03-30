using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using passman_back.Business.Dtos;
using passman_back.Infrastructure.Business.Validators;

namespace passman_back.IoC {
    public static class ValidatorModule {
        public static void ConfigureValidators(this IServiceCollection services) {
            services.AddTransient<IValidator<DirectoryCreateDto>, DirectoryCreateDtoValidator>();
            services.AddTransient<IValidator<DirectoryUpdateDto>, DirectoryUpdateDtoValidator>();

            services.AddTransient<IValidator<PasscardCreateDto>, PasscardCreateDtoValidator>();
            services.AddTransient<IValidator<PasscardUpdateDto>, PasscardUpdateDtoValidator>();
        }
    }
}
