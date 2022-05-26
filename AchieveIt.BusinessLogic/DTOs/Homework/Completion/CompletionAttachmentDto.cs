using AchieveIt.DataAccess.Entities;

namespace AchieveIt.BusinessLogic.DTOs.Homework.Completion
{
    public class CompletionAttachmentDto
    {
        public int HomeworkCompletionId { get; set; }
        
        public int FileAttachmentId { get; set; }
        
        public FileAttachment FileAttachment { get; set; }
    }
}