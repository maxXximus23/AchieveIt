using System.Threading.Tasks;
using AchieveIt.DataAccess.Repositories;
using AchieveIt.DataAccess.Repositories.Contracts;

namespace AchieveIt.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IUsersRepository _users;
        private IGroupRepository _group;
        private ISubjectRepository _subject;
        private IRefreshTokenRepository _refreshTokens;
        private IForumRepository _forums;
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

        public IGroupRepository Groups
        {
            get
            {
                if (_group is null)
                {
                    _group = new GroupRepository(_context);
                }

                return _group; 
            }
        }

        public ISubjectRepository Subjects
        {
            get
            {
                if (_subject is null)
                {
                    _subject = new SubjectRepository(_context);
                }

                return _subject;
            }
        }

        public IRefreshTokenRepository RefreshTokens
        {
            get
            {
                if (_refreshTokens is null)
                {
                    _refreshTokens = new RefreshTokenRepository(_context);
                }

                return _refreshTokens;
            }
        }

        public IForumRepository Forums
        {
            get
            {
                if (_forums is null)
                {
                    _forums = new ForumRepository(_context);
                }

                return _forums;
            }
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}