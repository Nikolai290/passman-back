using FluentValidation;

namespace passman_back.Infrastructure.Business.Validators.User {
    public class UserValidator : AbstractValidator<passman_back.Domain.Core.DbEntities.User> {
        public UserValidator() {
            RuleFor(x => x.IsBlocked)
                .Equal(false)
                .WithMessage("Пользователь заблокирован");
        }
    }
}
