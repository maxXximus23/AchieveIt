using AchieveIt.API.Models;
using AchieveIt.BusinessLogic.DTOs.Auth;
using AutoMapper;

namespace AchieveIt.API.Profiles
{
    public class SignInProfiles : Profile
    {
        public SignInProfiles()
        {
            CreateMap<SignInModel, SignInDto>();
        }
    }
}