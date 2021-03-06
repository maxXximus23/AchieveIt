using System.Threading.Tasks;
using AchieveIt.DataAccess.Repositories.Contracts;

namespace AchieveIt.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IUsersRepository Users { get; }
        
        public IGroupRepository Groups { get; }
        
        public ISubjectRepository Subjects { get; }
        
        public IHomeworkRepository Homeworks { get; }
        
        public IAchievementRepository Achievements { get; }
        
        public IHomeworkAttachmentRepository HomeworkAttachment { get; }
        
        public IRefreshTokenRepository RefreshTokens { get; }
        public IForumRepository Forums { get; }

        public Task SaveChanges();
    }
}