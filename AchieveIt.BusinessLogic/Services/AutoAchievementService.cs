using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.UnitOfWork;
using AchieveIt.Shared.Constants;

namespace AchieveIt.BusinessLogic.Services
{
    public class AutoAchievementService : IAutoAchievementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AutoAchievementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAddAttachmentEvent(int studentId, int attachmentsCount)
        {
            await HandleInternal(studentId, AchievementConstants.AddFirstAttachment);

            if (attachmentsCount >= 5)
            {
                await HandleInternal(studentId, AchievementConstants.PinFiveOrMoreHomeworkAttachmentsAtOnce);
            }
        }

        public async Task HandleAssessmentEvent(int studentId, int mark)
        {
            switch (mark)
            {
                case 100:
                    await HandleInternal(studentId, AchievementConstants.GetHundredMark);
                    break;
                case >= 75 and <= 90:
                    await HandleInternal(studentId, AchievementConstants.GetFourMark);
                    break;
                case <= 20:
                    await HandleInternal(studentId, AchievementConstants.GetLessThenTwentyMark);
                    break;
            }
        }

        public async Task HandleCreateForumTopicEvent(int studentId)
        {
            await HandleInternal(studentId, AchievementConstants.CreateFirstQuestionOnForum);
        }

        public async Task HandleCreateForumCommentEvent(int studentId)
        {
            await HandleInternal(studentId, AchievementConstants.AnswerFirstQuestionOnForum);
        }

        public async Task HandleSubmitHomeworkEvent(int studentId)
        {
            var studentHomeworksCount = await _unitOfWork.Homeworks.CountStudentHomeworks(studentId);

            if (studentHomeworksCount == 10)
            {
                await HandleInternal(studentId, AchievementConstants.SubmitTenHomeworks);
            }
        }

        private async Task HandleInternal(int studentId, int achievementId)
        {
            if (await FindAchievement(studentId, achievementId) is not null)
            {
                return;
            }
                
            await AddAchievement(studentId, achievementId);
            
            if (await FindAchievement(studentId, AchievementConstants.GetFirstAchievement) is null)
            {
                await AddAchievement(studentId, AchievementConstants.GetFirstAchievement);
            }
        }
        
        private async Task<AchievementUser> FindAchievement(int userId, int achievementId)
        {
            return await _unitOfWork.Achievements.GetStudentAchievement(achievementId, userId);
        }

        private async Task AddAchievement(int userId, int achievementId)
        {
            await _unitOfWork.Achievements.CreateStudentAchievement(new AchievementUser()
            {
                UserId = userId,
                AchievementId = achievementId
            });

            await _unitOfWork.SaveChanges();
        }
    }
}