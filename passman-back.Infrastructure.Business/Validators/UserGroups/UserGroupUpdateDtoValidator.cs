using FluentValidation;
using Microsoft.Extensions.Options;
using passman_back.Business.Dtos;
using passman_back.Infrastructure.Business.Settigns;

namespace passman_back.Infrastructure.Business.Validators {
    public class UserGroupUpdateDtoValidator : AbstractValidator<UserGroupUpdateDto> {

        private readonly PassmanSettings settings;
        public UserGroupUpdateDtoValidator(
            IOptions<PassmanSettings> options
        ) {
            settings = options.Value;
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Введите название папки")
                .NotNull()
                .WithMessage("Введите название папки")
                .Must(value => value.Length <= settings.MaxLengthOfTextFields)
                .WithMessage($"Название папки должно быть меньше {settings.MaxLengthOfTextFields} символов");
        }
    }
}
