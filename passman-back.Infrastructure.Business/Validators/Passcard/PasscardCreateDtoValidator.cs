using FluentValidation;
using Microsoft.Extensions.Options;
using passman_back.Business.Dtos;
using passman_back.Infrastructure.Business.Settigns;

namespace passman_back.Infrastructure.Business.Validators {
    public class PasscardCreateDtoValidator : AbstractValidator<PasscardCreateDto> {
        private readonly PassmanSettings settings;

        public PasscardCreateDtoValidator(
            IOptions<PassmanSettings> options
        ) {
            settings = options.Value;
            RuleFor(x => x.ParentIds)
                .NotEmpty()
                .NotNull()
                .WithMessage("Необходимо указать хотя бы одну папку");
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("Название пасскарды не может быть null")
                .Must(value => value != null && value.Length <= settings.MaxLengthOfTextFields)
                .WithMessage($"Название пасскарды должно быть меньше {settings.MaxLengthOfTextFields} символов");
        }
    }
}
