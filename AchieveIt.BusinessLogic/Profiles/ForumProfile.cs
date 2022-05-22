using AchieveIt.BusinessLogic.DTOs.Forum;
using AchieveIt.DataAccess.Entities.Forum;
using AutoMapper;

namespace AchieveIt.BusinessLogic.Profiles
{
    public class ForumProfile : Profile
    {
        public ForumProfile()
        {
            CreateMap<ForumTopic, TopicDto>();
            CreateMap<CreateTopicDto, ForumTopic>();
            CreateMap<AddCommentDto, ForumTopicComment>();
            CreateMap<UpdateCommentDto, ForumTopicComment>();
            CreateMap<ForumTopicComment, TopicCommentDto>();
        }
    }
}