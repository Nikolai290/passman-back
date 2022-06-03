﻿using FluentValidation;
using Microsoft.Extensions.Options;
using passman_back.Business.Dtos;
using passman_back.Infrastructure.Business.Settigns;

namespace passman_back.Infrastructure.Business.Validators {
    public class DirectoryUpdateDtoValidator : AbstractValidator<DirectoryUpdateDto> {
        private readonly PassmanSettings settings;

        public DirectoryUpdateDtoValidator(
            IOptions<PassmanSettings> options
        ) {
            settings = options.Value;
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Введите название папки")
                .Must(value => value.Length <= settings.MaxLengthOfTextFields)
                .WithMessage($"Название папки должно быть меньше {settings.MaxLengthOfTextFields} символов");
        }
    }
}
