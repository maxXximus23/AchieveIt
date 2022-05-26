using AchieveIt.DataAccess.Entities;

namespace AchieveIt.BusinessLogic.DTOs.Homework.Completion
{
    public class HomeworkCompletionDto
    {
        public int HomeworkId { get; set; }
        
        public int StudentId { get; set; }
        
        public int? Mark { get; set; }
        
        public CompletionAttachment[] CompletionAttachments { get; set; }
    }
}