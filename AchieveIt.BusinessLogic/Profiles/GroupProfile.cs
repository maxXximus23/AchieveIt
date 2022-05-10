using AchieveIt.BusinessLogic.DTOs.Group;
using AchieveIt.DataAccess.Entities;
using AutoMapper;

namespace AchieveIt.BusinessLogic.Profiles
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<GroupDto, Group>().ReverseMap();
            CreateMap<CreateGroupDto, GroupDto>();
            CreateMap<CreateGroupDto, Group>();
            CreateMap<UpdateGroupDto, Group>();
        }
    }
}