
namespace AchieveIt.DataAccess.Entities
{
    public class HomeworkFileAttachment : EntityBase<int>
    {
        public int FileAttachmentId { get; set; }
        
        public int HomeworkId { get; set; }
        
        public FileAttachment FileAttachment { get; set; }
        
        public Homework Homework { get; set; }
    }
}