using System.Linq;
using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.Repositories.Contracts;
using AchieveIt.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AchieveIt.DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DatabaseContext _context;

        public UsersRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id)
                ?? throw new NotFoundException($"User with id {id} has not found.");
        }

        public async Task<bool> IsEmailExist(string email)
        {
            return await _context.Users.AnyAsync(user => user.Email == email);
        }
    }
}