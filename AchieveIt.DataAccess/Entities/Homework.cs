using System.Collections.Generic;
using System.Net.Mail;

namespace AchieveIt.DataAccess.Entities
{
    public class Homework : EntityBase<int>
    {
        public string Title { get; set; }
        
        public int SubjectId { get; set; }
        
        public string Description { get; set; }
        
        public Subject Subject { get; set; }
        
        public List<HomeworkFileAttachment> Attachments { get; set; }

        public Homework()
        {
            Attachments = new List<HomeworkFileAttachment>();
        }
    }
}