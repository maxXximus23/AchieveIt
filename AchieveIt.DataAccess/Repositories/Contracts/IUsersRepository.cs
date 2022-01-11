using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;

namespace AchieveIt.DataAccess.Repositories.Contracts
{
    public interface IUsersRepository
    {
        public void AddUser(User user);

        public Task<bool> IsEmailExist(string email);
    }
}