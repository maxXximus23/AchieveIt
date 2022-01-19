using AchieveIt.API.Models;
using AchieveIt.BusinessLogic.DTOs.Auth;
using AutoMapper;

namespace AchieveIt.API.Profiles
{
    public class RefreshTokenProfile : Profile
    {
        public RefreshTokenProfile()
        {
            CreateMap<RefreshTokenModel, RefreshTokenDto>();
        }
    }
}