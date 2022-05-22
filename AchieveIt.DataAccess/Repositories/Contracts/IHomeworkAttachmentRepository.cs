using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;

namespace AchieveIt.DataAccess.Repositories.Contracts
{
    public interface IHomeworkAttachmentRepository
    {
        public Task<HomeworkFileAttachment> GetHomeworkAttachmentById(int homeworkAttachmentId);
        public void DeleteFileAttachments(FileAttachment fileAttachment);
    }
}