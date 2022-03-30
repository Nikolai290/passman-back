using FluentValidation;
using passman_back.Business.Dtos;

namespace passman_back.Infrastructure.Business.Validators {
    public class PasscardUpdateDtoValidator : AbstractValidator<PasscardUpdateDto> {
        public PasscardUpdateDtoValidator() {
            RuleFor(x => x.Login)
                .NotEmpty()
                .NotNull()
                .WithMessage("Введите логин для входа");
            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                .WithMessage("Введите пароль входа");
            RuleFor(x => x.Url)
                .NotEmpty()
                .NotNull()
                .WithMessage("Введите адрес ресурса");
            RuleFor(x => x.ParentIds)
                .NotEmpty()
                .NotNull()
                .WithMessage("Необходимо указать хотя бы одну папку");
        }
    }
}
