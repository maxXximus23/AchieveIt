using System.Collections.Generic;

namespace AchieveIt.DataAccess.Entities
{
    public class HomeworkCompletion : EntityBase<int>
    {
        public int HomeworkId { get; set; }
        
        public int StudentId { get; set; }
        
        public int? Mark { get; set; }
        
        public ICollection<CompletionAttachment> CompletionAttachments { get; set; }
        
        public Homework Homework { get; set; }

        public HomeworkCompletion()
        {
            CompletionAttachments = new List<CompletionAttachment>();
        }
    }
}