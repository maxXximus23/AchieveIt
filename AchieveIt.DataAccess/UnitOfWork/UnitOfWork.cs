using System.Threading.Tasks;
using AchieveIt.DataAccess.Repositories;
using AchieveIt.DataAccess.Repositories.Contracts;

namespace AchieveIt.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IUsersRepository _users;
        private readonly DatabaseContext _context;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }
        
        public IUsersRepository Users 
        {
            get
            {
                if (_users is null)
                {
                    _users = new UsersRepository(_context);
                }

                return _users;
            }
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}