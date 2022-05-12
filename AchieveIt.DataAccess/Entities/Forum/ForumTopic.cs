using System;
using System.Collections.Generic;

namespace AchieveIt.DataAccess.Entities.Forum
{
    public class ForumTopic : EntityBase<int>
    {
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public int AuthorId { get; set; }  
        public Student Author { get; set; }
        public ICollection<ForumTopicComment> Comments { get; set; }
    }
}