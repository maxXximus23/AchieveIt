using AchieveIt.API.Models;
using AchieveIt.BusinessLogic.DTOs.Auth;
using AchieveIt.DataAccess.Entities;
using AutoMapper;

namespace AchieveIt.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterStudentModel, RegisterStudentDto>();

            CreateMap<RegisterTeacherModel, RegisterTeacherDto>();
            
            CreateMap<RegisterAdminModel, RegisterAdminDto>();
        }
    }
}