using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;

namespace AchieveIt.DataAccess.Repositories.Contracts
{
    public interface IUsersRepository
    {
        public void AddUser(User user);

        public Task<TUser> GetUser<TUser>(int id)
            where TUser : User;

        public void UpdateUser(User user);

        public Task<TUser> GetUserByEmail<TUser>(string email)
            where TUser : User;

        public Task<bool> IsEmailExist(string email);
    }
}