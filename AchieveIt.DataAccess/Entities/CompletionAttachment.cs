using System.Collections.Generic;

namespace AchieveIt.DataAccess.Entities
{
    public class CompletionAttachment : EntityBase<int>
    {
        public int HomeworkCompletionId { get; set; }
        
        public int FileAttachmentId { get; set; }
        
        public FileAttachment FileAttachment { get; set; }
    }
}