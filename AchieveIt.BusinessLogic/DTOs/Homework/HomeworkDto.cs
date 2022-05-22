using AchieveIt.DataAccess.Entities;

namespace AchieveIt.BusinessLogic.DTOs.Homework
{
    public class HomeworkDto
    {
        public string Title { get; set; }
        
        public int SubjectId { get; set; }
        
        public string Description { get; set; }
        
        public FileAttachment[] Attachments { get; set; }
    }
}