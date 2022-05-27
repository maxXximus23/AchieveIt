using AchieveIt.BusinessLogic.DTOs.Achievement;
using AchieveIt.DataAccess.Entities;
using AutoMapper;

namespace AchieveIt.BusinessLogic.Profiles
{
    public class AchievementProfile : Profile
    {
        public AchievementProfile()
        {
            CreateMap<Achievement, AchievementDto>().ReverseMap();
            CreateMap<AchievementUser, AchievementUserDto>()
                .ForMember(achievementUserDto => achievementUserDto.StudentId,
                    rule => rule.MapFrom(achievement => achievement.UserId));
            CreateMap<CreateStudentAchievementDto, AchievementUser>();
            CreateMap<CreateAchievementDto, Achievement>()
                .ForMember(createAchievementDto => createAchievementDto.Url, 
                    rule => rule.Ignore());
            CreateMap<UpdateAchievementDto, Achievement>()
                .ForMember(createAchievementDto => createAchievementDto.Url, 
                    rule => rule.Ignore());
        }
    }
}