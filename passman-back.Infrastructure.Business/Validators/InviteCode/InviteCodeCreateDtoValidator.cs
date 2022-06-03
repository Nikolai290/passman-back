using FluentValidation;
using passman_back.Business.Dtos;
using System;

namespace passman_back.Infrastructure.Business.Validators {
    public class InviteCodeCreateDtoValidator : AbstractValidator<InviteCodeCreateDto> {
        public InviteCodeCreateDtoValidator() {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Введите название")
                .NotNull()
                .WithMessage("Введите название");
        }
    }
}
