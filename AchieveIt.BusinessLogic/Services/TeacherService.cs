using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Homework;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.UnitOfWork;
using AchieveIt.Shared.Exceptions;
using AchieveIt.Shared.Extensions;
using AutoMapper;
using Kirpichyov.FriendlyJwt;
using Kirpichyov.FriendlyJwt.Contracts;
using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IMapper _mapper;
        private readonly IJwtTokenReader _jwtTokenReader;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public TeacherService(IMapper mapper, IFileService fileService, IUnitOfWork unitOfWork,
            IJwtTokenReader jwtTokenReader)
        {
            _mapper = mapper;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _jwtTokenReader = jwtTokenReader;
        }

        public async Task<HomeworkDto> CreateHomework(int subjectId, CreateHomeworkDto createHomeworkDto)
        {
            Homework homework = _mapper.Map<CreateHomeworkDto, Homework>(createHomeworkDto);

            Task<FileAttachment>[] uploadTasks = createHomeworkDto.Attachments
                .Select(CreateAttachment)
                .ToArray();

            var fileAttachments = await Task.WhenAll(uploadTasks);
            var homeworkAttachments = fileAttachments.Select(file => new HomeworkFileAttachment()
            {
                FileAttachment = file
            });
            homework.Attachments.AddRange(homeworkAttachments);

            var subject = await _unitOfWork.Subjects.GetSubjectById(subjectId);
            if (subject.TeacherId != _jwtTokenReader.GetUserId())
            {
                throw new UnauthorizedAccessException("You dont have permission to add new homework!");
            }
            subject.Homeworks.Add(homework);
            
            await _unitOfWork.SaveChanges();

            return _mapper.Map<Homework, HomeworkDto>(homework);
        }

        public Task<IReadOnlyCollection<HomeworkDto>> GetAllHomework()
        {
            throw new System.NotImplementedException();
        }

        public Task<HomeworkDto> GetHomeworkById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<HomeworkDto> UpdateHomework(int id, UpdateHomeworkDto updateHomeworkDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<HomeworkDto> DeleteHomework(int id)
        {
            throw new System.NotImplementedException();
        }
        
        private async Task<FileAttachment> CreateAttachment(IFormFile homeworkAttachment)
        {
            string url = await _fileService.UploadFile(homeworkAttachment);
            return new FileAttachment
            {
                Url = url, 
                OriginalName = homeworkAttachment.FileName, 
                UploadTime = DateTime.UtcNow
            };
        }
    }
}