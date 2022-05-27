using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.DTOs.Achievement
{
    public class UpdateAchievementDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        public IFormFile Icon { get; set; }
    }
}