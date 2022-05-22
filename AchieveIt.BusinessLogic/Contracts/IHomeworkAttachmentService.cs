using System.Threading.Tasks;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface IHomeworkAttachmentService
    {
        public Task DeleteHomeworkAttachment(int homeworkAttachmentId);
    }
}