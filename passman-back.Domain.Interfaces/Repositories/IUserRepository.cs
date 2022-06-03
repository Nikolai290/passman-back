using passman_back.Domain.Core.DbEntities;
using System.Threading.Tasks;

namespace passman_back.Domain.Interfaces.Repositories {
    public interface IUserRepository : IBaseCrudRepository<User> {
        Task<User> GetByLoginAsync(string login);
    }
}
