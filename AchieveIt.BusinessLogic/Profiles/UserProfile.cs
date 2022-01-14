using System;
using AchieveIt.BusinessLogic.DTOs.Auth;
using AchieveIt.DataAccess.Entities;
using AutoMapper;

namespace AchieveIt.BusinessLogic.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserDto, User>()
                .ForMember(user => user.Password, 
                    opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)))
                .ForMember(user => user.Role, 
                    opt => opt.MapFrom(src => MapRole(src)))
                .IncludeAllDerived();

            CreateMap<RegisterTeacherDto, Teacher>();
            CreateMap<RegisterAdminDto, Admin>();
            CreateMap<RegisterStudentDto, Student>();
        }

        private Role MapRole(RegisterUserDto userDto)
        {
            return userDto switch
            {
                RegisterAdminDto => Role.Admin,
                RegisterTeacherDto => Role.Teacher,
                RegisterStudentDto => Role.Student,
                _ => throw new ArgumentOutOfRangeException(nameof(userDto))
            };
        }
    }
}