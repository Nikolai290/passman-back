using FluentValidation;
using Microsoft.Extensions.Options;
using passman_back.Business.Dtos;
using passman_back.Infrastructure.Business.Settigns;

namespace passman_back.Infrastructure.Business.Validators.User {
    public class UserRestorePasswordDtoValidator : AbstractValidator<UserRestorePasswordStepTwoDto> {
        private readonly PassmanSettings settings;
        public UserRestorePasswordDtoValidator(
            IOptions<PassmanSettings> options
        ) {
            this.settings = options.Value;
            RuleFor(x => x.RestorePasswordCode)
                .NotEmpty()
                .WithMessage("Введите секретный код")
                .NotNull()
                .WithMessage("Введите секретный код");
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Введите новый пароль")
                .NotNull()
                .WithMessage("Введите новый пароль")
                .Must(value => value.Length >= settings.MinPasswordLengthForUsers)
                .WithMessage($"Длинна пароля должна быть не меньше {settings.MinPasswordLengthForUsers} символов");
            RuleFor(x => x.ConfirmPassword)
                .Must((dto, confirm) => dto.ConfirmPassword == dto.NewPassword)
                .WithMessage("Пароли не совпадают");
        }
    }
}
