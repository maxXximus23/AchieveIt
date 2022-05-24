using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.DTOs.Homework.Completion
{
    public class UpdateHomeworkCompletionDto
    {
        public IFormFile[] Files { get; set; }
    }
}