using System.Collections.Generic;
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

        public async Task<TUser> GetUser<TUser>(int id)
        where TUser : User
        {
            return await _context.Users.OfType<TUser>().FirstOrDefaultAsync(user => user.Id == id)
                ?? throw new NotFoundException($"{typeof(TUser).Name} with id {id} has not found.");
        }

        public async Task<TUser> GetUserByEmail<TUser>(string email)
        where TUser : User
        {
            return await _context.Users.OfType<TUser>()
                .AsNoTracking()
                .SingleOrDefaultAsync(user => user.Email == email)
                   ?? throw new NotFoundException($"User with email:{email} has not found.");
        }

        public async Task<bool> IsEmailExist(string email)
        {
            return await _context.Users.AnyAsync(user => user.Email == email);
        }
        
        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }
    }
}