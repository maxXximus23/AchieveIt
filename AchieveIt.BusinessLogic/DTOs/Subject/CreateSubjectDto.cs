using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.DTOs.Subject
{
    public class CreateSubjectDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int GroupId { get; set; }
        
        public int TeacherId { get; set; }
        
        public int AssistTeacherId { get; set; }
        
        public IFormFile Icon { get; set; }
    }
}