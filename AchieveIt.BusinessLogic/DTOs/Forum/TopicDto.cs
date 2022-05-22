using System;

namespace AchieveIt.BusinessLogic.DTOs.Forum
{
    public class TopicDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public int AuthorId { get; set; }
    }
}