using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.DTOs.Subject
{
    public class UpdateSubjectDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int GroupId { get; set; }
        
        public int TeacherId { get; set; }
        
        public int AssistTeacherId { get; set; }
        
        public IFormFile Icon { get; set; }
    }
}