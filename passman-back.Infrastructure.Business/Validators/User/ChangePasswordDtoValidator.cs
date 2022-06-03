using FluentValidation;
using Microsoft.Extensions.Options;
using passman_back.Business.Dtos;
using passman_back.Infrastructure.Business.Settigns;

namespace passman_back.Infrastructure.Business.Validators.User {
    public class ChangePasswordDtoValidator : AbstractValidator<UserChangePasswordDto> {
        private readonly PassmanSettings settings;
        public ChangePasswordDtoValidator(
            IOptions<PassmanSettings> options
        ) {
            this.settings = options.Value;
            RuleFor(x => x.OldPassword)
                .NotEmpty()
                .WithMessage("Введите старый пароль")
                .NotNull()
                .WithMessage("Введите старый пароль");
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Введите нвоый пароль")
                .NotNull()
                .WithMessage("Введите нвоый пароль")
                .Must(value => value.Length >= settings.MinPasswordLengthForUsers)
                .WithMessage($"Длинна пароля должна быть не меньше {settings.MinPasswordLengthForUsers} символов");
            RuleFor(x => x.ConfirmPassword)
                .Must((dto, confirm) => dto.ConfirmPassword == dto.NewPassword)
                .WithMessage("Пароли не совпадают");
        }
    }
}
