using FluentValidation;
using passman_back.Business.Dtos;

namespace passman_back.Infrastructure.Business.Validators {
    public class InviteCodeUpdateDtoValidator : AbstractValidator<InviteCodeUpdateDto> {
        public InviteCodeUpdateDtoValidator() {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Введите название")
                .NotNull()
                .WithMessage("Введите название");
        }
    }
}
