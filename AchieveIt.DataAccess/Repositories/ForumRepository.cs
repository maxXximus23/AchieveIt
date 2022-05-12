using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities.Forum;
using AchieveIt.DataAccess.Repositories.Contracts;
using AchieveIt.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AchieveIt.DataAccess.Repositories
{
    public class ForumRepository : IForumRepository
    {
        private readonly DatabaseContext _databaseContext;

        public ForumRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IReadOnlyCollection<ForumTopic>> GetTopics()
        {
            return await _databaseContext.ForumTopics.AsNoTracking().ToArrayAsync();
        }

        public Task<ForumTopic> GetTopic(int id)
        {
            return _databaseContext.ForumTopics.SingleOrDefaultAsync(topic => topic.Id == id)
                ?? throw new NotFoundException(nameof(ForumTopic), id.ToString());
        }

        public void AddTopic(ForumTopic topic)
        {
            _databaseContext.ForumTopics.Add(topic);
        }

        public void DeleteTopic(ForumTopic topic)
        {
            _databaseContext.ForumTopics.Remove(topic);
        }

        public async Task<IReadOnlyCollection<ForumTopicComment>> GetComments(int topicId)
        {
            return await _databaseContext.ForumTopicComments
                .AsNoTracking()
                .Where(comment => comment.ForumTopicId == topicId)
                .ToArrayAsync();
        }

        public async Task<ForumTopicComment> GetComment(int commentId)
        {
            return await _databaseContext.ForumTopicComments.SingleOrDefaultAsync(comment => comment.Id == commentId)
                   ?? throw new NotFoundException(nameof(ForumTopicComment), commentId.ToString());
        }

        public void AddComment(ForumTopicComment comment)
        {
            _databaseContext.ForumTopicComments.Add(comment);
        }

        public void DeleteComment(ForumTopicComment comment)
        {
            _databaseContext.ForumTopicComments.Remove(comment);
        }
    }
}