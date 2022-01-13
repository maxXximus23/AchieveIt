using AchieveIt.BusinessLogic.DTOs.Auth;
using AchieveIt.DataAccess.Entities;
using AutoMapper;

namespace AchieveIt.BusinessLogic.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterStudentDto, Student>()
                .ForMember(user => user.Password, opt => opt.MapFrom(src =>
                BCrypt.Net.BCrypt.HashPassword(src.Password)))
                .ForMember(user => user.Role, opt => opt.MapFrom(src => Role.Student));

            CreateMap<RegisterTeacherDto, Teacher>()
                .ForMember(teacher => teacher.Password, opt => opt.MapFrom(src =>
                BCrypt.Net.BCrypt.HashPassword(src.Password)))
                .ForMember(user => user.Role, opt => opt.MapFrom(src => Role.Teacher));
            
            CreateMap<RegisterAdminDto, Admin>()
                .ForMember(admin => admin.Password, opt => opt.MapFrom(src =>
                BCrypt.Net.BCrypt.HashPassword(src.Password)))
                .ForMember(user => user.Role, opt => opt.MapFrom(src => Role.Admin));
        }
    }
}