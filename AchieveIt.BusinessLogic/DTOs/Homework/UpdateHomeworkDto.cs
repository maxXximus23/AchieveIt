using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.DTOs.Homework
{
    public class UpdateHomeworkDto
    {
        public string Title { get; set; }
        
        public int SubjectId { get; set; }
        
        public string Description { get; set; }
    }
}