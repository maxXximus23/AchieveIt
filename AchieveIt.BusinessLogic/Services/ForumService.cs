using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Forum;
using AchieveIt.BusinessLogic.Extensions;
using AchieveIt.DataAccess.Entities.Forum;
using AchieveIt.DataAccess.UnitOfWork;
using AutoMapper;
using Kirpichyov.FriendlyJwt.Contracts;

namespace AchieveIt.BusinessLogic.Services
{
    public class ForumService : IForumService
    {
        private readonly IJwtTokenReader _tokenReader;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAutoAchievementService _autoAchievementService;

        public ForumService(IJwtTokenReader tokenReader, IUnitOfWork unitOfWork, IMapper mapper, IAutoAchievementService autoAchievementService)
        {
            _tokenReader = tokenReader;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _autoAchievementService = autoAchievementService;
        }

        public async Task<TopicDto> CreateTopic(CreateTopicDto dto)
        {
            var topic = _mapper.Map<CreateTopicDto, ForumTopic>(dto);
            topic.CreationDate = DateTime.UtcNow;
            topic.AuthorId = _tokenReader.GetUserId();

            _unitOfWork.Forums.AddTopic(topic);
            await _unitOfWork.SaveChanges();

            await _autoAchievementService.HandleCreateForumTopicEvent(_tokenReader.GetUserId());
            
            return _mapper.Map<ForumTopic, TopicDto>(topic);
        }

        public async Task<IReadOnlyCollection<TopicDto>> GetTopics()
        {
            var topics = await _unitOfWork.Forums.GetTopics();
            return _mapper.Map<IReadOnlyCollection<ForumTopic>, IReadOnlyCollection<TopicDto>>(topics);
        }

        public async Task<TopicDto> GetTopic(int id)
        {
            var topic = await _unitOfWork.Forums.GetTopic(id);
            return _mapper.Map<ForumTopic, TopicDto>(topic);
        }

        public async Task DeleteTopic(int id)
        {
            var topic = await _unitOfWork.Forums.GetTopic(id);

            if (topic.AuthorId != _tokenReader.GetUserId())
            {
                throw new UnauthorizedAccessException();
            }
            
            _unitOfWork.Forums.DeleteTopic(topic);
            await _unitOfWork.SaveChanges();
        }

        public async Task<TopicCommentDto> AddComment(int topicId, AddCommentDto dto)
        {
            var topic = await _unitOfWork.Forums.GetTopic(topicId);
            
            var comment =_mapper.Map<AddCommentDto, ForumTopicComment>(dto);
            comment.CreationDate = DateTime.UtcNow;
            comment.AuthorId = _tokenReader.GetUserId();
            
            topic.Comments.Add(comment);
            await _unitOfWork.SaveChanges();

            await _autoAchievementService.HandleCreateForumCommentEvent(_tokenReader.GetUserId());
            
            return _mapper.Map<ForumTopicComment, TopicCommentDto>(comment);
        }

        public async Task<IReadOnlyCollection<TopicCommentDto>> GetComments(int topicId)
        {
            var comments = await _unitOfWork.Forums.GetComments(topicId);
            return _mapper.Map<IReadOnlyCollection<ForumTopicComment>, IReadOnlyCollection<TopicCommentDto>>(comments); 
        }

        public async Task<TopicCommentDto> GetComment(int commentId)
        {
            var comment = await _unitOfWork.Forums.GetComment(commentId);
            return _mapper.Map<ForumTopicComment, TopicCommentDto>(comment); 
        }

        public async Task<TopicCommentDto> UpdateComment(int commentId, UpdateCommentDto dto)
        {
            var comment = await _unitOfWork.Forums.GetComment(commentId);

            if (comment.AuthorId != _tokenReader.GetUserId())
            {
                throw new UnauthorizedAccessException();
            }

            _mapper.Map(dto, comment);
            comment.ModificationDate = DateTime.UtcNow;
            await _unitOfWork.SaveChanges();

            return _mapper.Map<ForumTopicComment, TopicCommentDto>(comment);
        }

        public async Task DeleteComment(int commentId)
        {
            var comment = await _unitOfWork.Forums.GetComment(commentId);

            if (comment.AuthorId != _tokenReader.GetUserId())
            {
                throw new UnauthorizedAccessException();
            }
            
            _unitOfWork.Forums.DeleteComment(comment);
            await _unitOfWork.SaveChanges();
        }
    }
}