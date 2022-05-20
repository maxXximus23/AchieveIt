using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.DTOs.Homework
{
    public class UploadHomeworkAttachmentDto
    {
        public IFormFile File { get; set; }
    }
}