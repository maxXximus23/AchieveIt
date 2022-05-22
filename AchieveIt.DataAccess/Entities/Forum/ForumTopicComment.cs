using System;

namespace AchieveIt.DataAccess.Entities.Forum
{
    public class ForumTopicComment : EntityBase<int>
    {
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int ForumTopicId { get; set; }
        public int AuthorId { get; set; }

        public ForumTopic ForumTopic { get; set; }
        public Student Author { get; set; }
    }
}