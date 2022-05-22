using AchieveIt.DataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.DTOs.Homework
{
    public class CreateHomeworkDto
    {
        public string Title { get; set; }

        public string Description { get; set; }
        
        public IFormFile[] Attachments { get; set; }
    }
}