using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.DTOs.Homework.Completion
{
    public class UploadHomeworkCompletionDto
    {
        public int StudentId { get; set; }
        
        public IFormFile[] Files { get; set; }
    }
}