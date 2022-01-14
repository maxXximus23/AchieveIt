using AchieveIt.BusinessLogic.DTOs.Auth;
using AchieveIt.DataAccess.Entities;
using AutoMapper;

namespace AchieveIt.BusinessLogic.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleDto, Role>();
        }
    }
}