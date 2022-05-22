using System;
using System.Net.Mail;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Homework;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.UnitOfWork;
using AchieveIt.Shared.Extensions;
using AutoMapper;
using Kirpichyov.FriendlyJwt.Contracts;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.Services
{
    public class HomeworkService : IHomeworkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IJwtTokenReader _jwtTokenReader;
        private readonly IMapper _mapper;

        public HomeworkService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, 
            IJwtTokenReader jwtTokenReader)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _jwtTokenReader = jwtTokenReader;
        }

        public async Task<HomeworkDto> GetHomeworkById(int id)
        {
            var homework = await _unitOfWork.Homeworks.GetHomeworkById(id);
            return _mapper.Map<Homework, HomeworkDto>(homework);
        }

        public async Task DeleteHomeworkById(int id)
        {
            await _unitOfWork.Homeworks.DeleteHomeworkById(id);
            await _unitOfWork.SaveChanges();
        }

        public async Task<HomeworkDto> AddHomeworkAttachment(int homeworkId, IFormFile file)
        {
            var homework = await _unitOfWork.Homeworks.GetHomeworkById(homeworkId, true);
            
            if (homework.Subject.TeacherId != _jwtTokenReader.GetUserId())
            {
                throw new UnauthorizedAccessException("You dont have permission to add new homework!");
            }

            var url = await _fileService.UploadFile(file);
            FileAttachment fileAttachment = new FileAttachment
            {
                Url = url,
                UploadTime = DateTime.UtcNow,
                OriginalName = file.FileName
            };

            var homeworkAttachments = new HomeworkFileAttachment()
            {
                FileAttachment = fileAttachment
            };
            
            homework.Attachments.Add(homeworkAttachments);
            await _unitOfWork.SaveChanges();

            return _mapper.Map<Homework, HomeworkDto>(homework);
        }

        public async Task<HomeworkDto> UpdateHomework(UpdateHomeworkDto updateHomeworkDto, int homeworkId)
        {
            Homework homework = await _unitOfWork.Homeworks.GetHomeworkById(homeworkId);

            await _unitOfWork.Subjects.GetSubjectById(updateHomeworkDto.SubjectId);

            _mapper.Map(updateHomeworkDto, homework);

            _unitOfWork.Homeworks.UpdateHomework(homework);
            await _unitOfWork.SaveChanges();

            return _mapper.Map<Homework, HomeworkDto>(homework);
        }
    }
}