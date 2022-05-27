using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.DTOs.Achievement;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface IAchievementService
    {
        public Task<AchievementDto> CreateAchievement(CreateAchievementDto createAchievementDto);

        public Task<AchievementUserDto> CreateStudentAchievement(
            CreateStudentAchievementDto createStudentAchievementDto, int studentId);

        public Task<AchievementDto> UpdateAchievement(UpdateAchievementDto updateAchievementDto, int achievementId);

        public Task DeleteAchievementById(int achievementId);

        public Task DeleteStudentAchievementById(DeleteStudentAchievementDto deleteStudentAchievementDto, 
            int studentId);

        public Task<IReadOnlyCollection<AchievementDto>> GetAchievements();
        
        public Task<AchievementDto> GetAchievementById(int achievementId);

        public Task<IReadOnlyCollection<AchievementDto>> GetStudentAchievements(int studentId);
    }
}