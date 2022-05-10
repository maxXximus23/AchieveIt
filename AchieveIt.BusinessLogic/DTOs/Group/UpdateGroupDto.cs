using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.DTOs.Group
{
    public class UpdateGroupDto
    {
        public string Name { get; set; }

        public string Faculty { get; set; }

        public string Department { get; set; }
        
        public IFormFile Avatar { get; set; }
    }
}