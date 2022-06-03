using FluentValidation;
using Microsoft.AspNetCore.Http;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.Repositories;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.Services {
    public class BaseService {
        protected readonly IUserRepository userRepository;
        protected readonly HttpContext httpContext;
        protected readonly IValidator<User> userValidator;


        public BaseService(
            IUserRepository userRepository,
            IHttpContextAccessor accessor,
            IValidator<User> userValidator
        ) {
            this.userRepository = userRepository;
            this.httpContext = accessor.HttpContext;
            this.userValidator = userValidator;
        }

        protected async Task<User> GetCurrentUserAsync()
            => await userRepository.GetByLoginAsync(httpContext.User.Identity.Name);
        protected async Task CheckUserIsBlocked()
            => await userValidator.ValidateAndThrowAsync(await GetCurrentUserAsync());
        protected async Task CheckUserIsBlocked(User user)
            => await userValidator.ValidateAndThrowAsync(user);

        protected async Task<User> GetCurrentUserAndValidateAsync() {
            var user = await userRepository.GetByLoginAsync(httpContext.User.Identity.Name);
            await userValidator.ValidateAndThrowAsync(user);
            return user;
        }
    }
}