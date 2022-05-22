using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities.Forum;

namespace AchieveIt.DataAccess.Repositories.Contracts
{
    public interface IForumRepository
    {
        Task<IReadOnlyCollection<ForumTopic>> GetTopics();
        Task<ForumTopic> GetTopic(int id);
        void AddTopic(ForumTopic topic);
        void DeleteTopic(ForumTopic topic);
        Task<IReadOnlyCollection<ForumTopicComment>> GetComments(int topicId);
        Task<ForumTopicComment> GetComment(int commentId);
        void AddComment(ForumTopicComment comment);
        void DeleteComment(ForumTopicComment comment);
    }
}