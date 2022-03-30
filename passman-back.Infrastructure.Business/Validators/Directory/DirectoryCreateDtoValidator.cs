using FluentValidation;
using passman_back.Business.Dtos;

namespace passman_back.Infrastructure.Business.Validators {
    public class DirectoryCreateDtoValidator : AbstractValidator<DirectoryCreateDto> {
        public DirectoryCreateDtoValidator() {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Введите название папки");
        }
    }
}
