using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.DTOs.Forum;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface IForumService
    {
        Task<TopicDto> CreateTopic(CreateTopicDto dto);
        Task<IReadOnlyCollection<TopicDto>> GetTopics();
        Task<TopicDto> GetTopic(int id);
        Task DeleteTopic(int id);

        Task<TopicCommentDto> AddComment(int topicId, AddCommentDto dto);
        Task<IReadOnlyCollection<TopicCommentDto>> GetComments(int topicId);
        Task<TopicCommentDto> GetComment(int commentId);
        Task<TopicCommentDto> UpdateComment(int commentId, UpdateCommentDto dto);
        Task DeleteComment(int commentId);
    }
}