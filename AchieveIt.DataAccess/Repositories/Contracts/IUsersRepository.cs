using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;

namespace AchieveIt.DataAccess.Repositories.Contracts
{
    public interface IUsersRepository
    {
        public void AddUser(User user);
        public Task<User> GetUser(int id);

        public Task<User> GetUserByEmail(string email);

        public Task<bool> IsEmailExist(string email);
    }
}