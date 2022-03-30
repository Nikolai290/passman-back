using FluentValidation;
using passman_back.Business.Dtos;

namespace passman_back.Infrastructure.Business.Validators {
    public class DirectoryUpdateDtoValidator : AbstractValidator<DirectoryUpdateDto> {
        public DirectoryUpdateDtoValidator() {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Введите название папки");
        }
    }
}
