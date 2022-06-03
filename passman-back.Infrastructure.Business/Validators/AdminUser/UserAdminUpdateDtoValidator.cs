using FluentValidation;
using Microsoft.Extensions.Options;
using passman_back.Business.Dtos;
using passman_back.Domain.Interfaces.Repositories;
using passman_back.Infrastructure.Business.Settigns;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Validators {
    public class UserAdminUpdateDtoValidator : AbstractValidator<UserAdminUpdateDto> {
        private readonly IUserRepository userRepository;
        private readonly PassmanSettings settings;
        private bool isUniqueNickname;
        private bool isUniqueEmail;

        public UserAdminUpdateDtoValidator(
            IUserRepository userRepository,
            IOptions<PassmanSettings> options
        ) {
            this.userRepository = userRepository;
            this.settings = options.Value;
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Введите действительный Id");

            var emailPattern
                = "^([a-z0-9_-]+\\.)*[a-z0-9_-]+@[a-z0-9_-]+(\\.[a-z0-9_-]+)*\\.[a-z]{2,6}$";
            RuleFor(x => x)
                .MustAsync(async (dto, token) => await IsUnique(dto, token));
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
                .WithMessage("Введите никнейм")
                .NotNull()
                .WithMessage("Введите никнейм")
                .Must(x => isUniqueNickname)
                .WithMessage("Этот ник уже занят")
                .Must(x => x.Length <= settings.MaxLengthOfTextFields)
                .WithMessage($"Ник должен быть короче {settings.MaxLengthOfTextFields} символов");
            RuleFor(x => x.FirstName)
                .NotNull()
                .WithMessage("Введите имя")
                .NotEmpty()
                .WithMessage("Введите имя");
            RuleFor(x => x.LastName)
                .NotNull()
                .WithMessage("Введите фамилию")
                .NotEmpty()
                .WithMessage("Введите фамилию");
        }

        private async Task<bool> IsUnique(UserAdminUpdateDto dto, CancellationToken token) {
            var users = await userRepository.GetAllAsync();
            var user = users.Single(x => x.Id == dto.Id);
            var anotherUsers = users.Where(x => x.Id != dto.Id);
            isUniqueNickname = anotherUsers.All(x => x.Nickname != dto.Nickname);
            isUniqueEmail = anotherUsers.All(x => x.Email.ToLower() != dto.Email.ToLower());

            return isUniqueNickname && isUniqueEmail;
        }
    }
}
