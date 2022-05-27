using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;

namespace AchieveIt.DataAccess.Repositories.Contracts
{
    public interface IAchievementRepository
    {
        public Task CreateAchievement(Achievement achievement);

        public Task CreateStudentAchievement(AchievementUser achievementUser);

        public void UpdateAchievement(Achievement achievement);

        public Task DeleteAchievementById(int achievementId);
        
        public Task DeleteStudentAchievement(int achievementId, int studentId);

        public Task<bool> IsAchievementExist(string achievementName);

        public Task<bool> IsStudentHaveAchievement(int studentId, int achievementId);

        public Task<IReadOnlyCollection<Achievement>> GetAchievements();

        public Task<Achievement> GetAchievementByName(string achievementName);
        
        public Task<Achievement> GetAchievementById(int achievementId);
        
        public Task<AchievementUser> GetStudentAchievement(int achievementId, int studentId);

        public Task<IReadOnlyCollection<Achievement>> GetStudentAchievements(int studentId);
    }
}