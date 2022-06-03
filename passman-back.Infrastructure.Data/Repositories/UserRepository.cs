using Microsoft.EntityFrameworkCore;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Interfaces.DbContexts;
using passman_back.Domain.Interfaces.Repositories;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Domain.Repositories {
    public class UserRepository : BaseCrudRepository<User>, IUserRepository {
        public UserRepository(IMainDbContext dbContext) : base(dbContext) {
        }

        public async Task<User> GetByLoginAsync(string login) {
            var users = await dbContext.Users.Where(x => !x.IsDeleted).ToListAsync();
            var emailPattern
                = "^([a-z0-9_-]+\\.)*[a-z0-9_-]+@[a-z0-9_-]+(\\.[a-z0-9_-]+)*\\.[a-z]{2,6}$";
            var loginLower = login.ToLower();

            var isEmail = Regex.IsMatch(loginLower, emailPattern);
            if (isEmail) {
                return users.Single(x => x.Email.ToLower().Equals(loginLower));
            }
            return users.Single(x => x.Nickname.Equals(login));
        }
    }
}
