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
        private IHomeworkRepository _homework;
        private IHomeworkAttachmentRepository _homeworkAttachment;
        private IRefreshTokenRepository _refreshTokens;
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

        public IHomeworkRepository Homeworks
        {
            get
            {
                if (_homework is null)
                {
                    _homework = new HomeworkRepository(_context);
                }

                return _homework;
            }
        }

        public IHomeworkAttachmentRepository HomeworkAttachment
        {
            get
            {
                if (_homeworkAttachment is null)
                {
                    _homeworkAttachment = new HomeworkAttachmentRepository(_context);
                }

                return _homeworkAttachment;
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

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}