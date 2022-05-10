using System.Threading.Tasks;
using AchieveIt.DataAccess.Repositories.Contracts;

namespace AchieveIt.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IUsersRepository Users { get; }
        
        public IGroupRepository Groups { get; }
        
        public IRefreshTokenRepository RefreshTokens { get; }

        public Task SaveChanges();
    }
}