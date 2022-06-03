using FluentValidation;
using Microsoft.Extensions.Options;
using passman_back.Business.Dtos;
using passman_back.Domain.Interfaces.Repositories;
using passman_back.Infrastructure.Business.Settigns;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Validators.User {
    public class RegisterDtoValidator : AbstractValidator<UserRegisterDto> {
        private readonly IUserRepository userRepository;
        private readonly PassmanSettings settings;
        private bool isUniqueNickname;
        private bool isUniqueEmail;

        public RegisterDtoValidator(
            IUserRepository userRepository,
            IOptions<PassmanSettings> options
        ) {
            this.userRepository = userRepository;
            this.settings = options.Value;
            var emailPattern
                = "^([a-z0-9_-]+\\.)*[a-z0-9_-]+@[a-z0-9_-]+(\\.[a-z0-9_-]+)*\\.[a-z]{2,6}$";
            RuleFor(x => x)
                .MustAsync(async (dto, token) => await IsUnique(dto, token))
                .WithMessage("Пользователь уже существует, авторизуйтесь");
            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                .WithMessage("Введите пароль")
                .Must(value => value.Length >= settings.MinPasswordLengthForUsers)
                .WithMessage($"Длинна пароля должна быть не меньше {settings.MinPasswordLengthForUsers} символво");
            RuleFor(x => x.ConfirmPassword)
                .Must((dto, confirm) => dto.ConfirmPassword == dto.Password)
                .WithMessage("Пароли не совпадают");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Введите электронную почту")
                .NotNull()
                .WithMessage("Введите электронную почту")
                .Must(email => Regex.IsMatch(email.ToLower(), emailPattern))
                .WithMessage("Введите электронную почту")
                .Must(x => isUniqueEmail)
                .WithMessage("Эта почта уже используется");
            RuleFor(x => x.Nickname)
                .NotEmpty()
                .NotNull()
                .WithMessage("Введит никнейм")
                .Must(x => isUniqueNickname)
                .WithMessage("Этот ник уже занят")
                .Must(x => x.Length <= settings.MaxLengthOfTextFields)
                .WithMessage($"Ник должен быть короче {settings.MaxLengthOfTextFields} символов");
            RuleFor(x => x.FirstName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Введите имя");
            RuleFor(x => x.LastName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Введите фамилию");


        }

        private async Task<bool> IsUnique(UserRegisterDto dto, CancellationToken token) {
            var users = await userRepository.GetAllAsync();
            isUniqueNickname = users.All(x => x.Nickname != dto.Nickname);
            isUniqueEmail = users.All(x => x.Email.ToLower() != dto.Email.ToLower());

            return isUniqueNickname && isUniqueEmail;
        }
    }
}
