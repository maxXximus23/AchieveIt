using System.Threading.Tasks;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface IAutoAchievementService
    {
        public Task HandleAddAttachmentEvent(int studentId, int attachmentsCount);
        Task HandleAssessmentEvent(int studentId, int mark);
        Task HandleCreateForumTopicEvent(int studentId);
        Task HandleCreateForumCommentEvent(int studentId);
        Task HandleSubmitHomeworkEvent(int studentId);
    }
}