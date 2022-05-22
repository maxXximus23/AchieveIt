using System;

namespace AchieveIt.BusinessLogic.DTOs.Forum
{
    public class TopicCommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int AuthorId { get; set; }
    }
}