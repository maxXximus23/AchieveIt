using AchieveIt.BusinessLogic.DTOs.Homework;
using AchieveIt.BusinessLogic.DTOs.Homework.Completion;
using AchieveIt.DataAccess.Entities;
using AutoMapper;

namespace AchieveIt.BusinessLogic.Profiles
{
    public class HomeworkProfile : Profile
    {
        public HomeworkProfile()
        {
            CreateMap<Homework, HomeworkDto>().ReverseMap();
            CreateMap<HomeworkCompletion, HomeworkCompletionDto>().ReverseMap();
            CreateMap<HomeworkFileAttachment, FileAttachment>()
                .ForMember(homeworkFileAttachment => homeworkFileAttachment.Url, 
                    rule => rule.MapFrom(fileAttachment => fileAttachment.FileAttachment.Url))
                .ForMember(homeworkFileAttachment => homeworkFileAttachment.OriginalName, 
                    rule => rule.MapFrom(fileAttachment => fileAttachment.FileAttachment.OriginalName))
                .ForMember(homeworkFileAttachment => homeworkFileAttachment.UploadTime, 
                    rule => rule.MapFrom(fileAttachment => fileAttachment.FileAttachment.UploadTime));
            CreateMap<CompletionAttachment, FileAttachment>()
                .ForMember(homeworkFileAttachment => homeworkFileAttachment.Url, 
                    rule => rule.MapFrom(fileAttachment => fileAttachment.FileAttachment.Url))
                .ForMember(homeworkFileAttachment => homeworkFileAttachment.OriginalName, 
                    rule => rule.MapFrom(fileAttachment => fileAttachment.FileAttachment.OriginalName))
                .ForMember(homeworkFileAttachment => homeworkFileAttachment.UploadTime, 
                    rule => rule.MapFrom(fileAttachment => fileAttachment.FileAttachment.UploadTime));
            CreateMap<CreateHomeworkDto, HomeworkDto>();
            CreateMap<CreateHomeworkDto, Homework>()
                .ForMember(createHomeworkDto => createHomeworkDto.Attachments, 
                    rule => rule.Ignore());
            CreateMap<UpdateHomeworkDto, Homework>();
            CreateMap<HomeworkDto, Homework>()
                .ForMember(createHomeworkDto => createHomeworkDto.Attachments, 
                    rule => rule.Ignore())
                .ReverseMap();
            CreateMap<UpdateHomeworkDto, Homework>();
        }
    }
}