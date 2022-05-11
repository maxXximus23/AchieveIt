using AchieveIt.BusinessLogic.DTOs.Subject;
using AchieveIt.BusinessLogic.DTOs.User.Teacher;
using AchieveIt.DataAccess.Entities;
using AutoMapper;

namespace AchieveIt.BusinessLogic.Profiles
{
    public class SubjectProfile : Profile
    {
        public SubjectProfile()
        {
            CreateMap<Subject, SubjectDto>().ReverseMap();
            CreateMap<CreateSubjectDto, SubjectDto>();
            CreateMap<CreateSubjectDto, Subject>();
            CreateMap<UpdateSubjectDto, Subject>();
            CreateMap<Subject, DetailedSubjectDto>();
            CreateMap<Teacher, SubjectTeacherDto>()
                .ForMember(teacherDto => teacherDto.Fullname, 
                    rule => rule.MapFrom(teacher => $"{teacher.Name} {teacher.Surname} {teacher.Patronymic}"));
        }
    }
}