using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Homework;
using AchieveIt.BusinessLogic.DTOs.Homework.Completion;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.UnitOfWork;
using AchieveIt.Shared.Exceptions;
using AchieveIt.Shared.Extensions;
using AutoMapper;
using Kirpichyov.FriendlyJwt.Contracts;
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

        public async Task<IReadOnlyCollection<HomeworkCompletionDto>> GetHomeworkCompletions(int homeworkId)
        {
            var homework = await _unitOfWork.Homeworks.GetHomeworkById(homeworkId);
            var user = await _unitOfWork.Users.GetUser<Teacher>(_jwtTokenReader.GetUserId());

            if (homework.Subject.GroupId != user.GroupId)
            {
                throw new ValidationException("You dont have permission to watch homework completion.");
            }

            var homeworkCompletions = await _unitOfWork.Homeworks.GetHomeworkCompletions(homeworkId);
            return _mapper.Map<IReadOnlyCollection<HomeworkCompletion>, IReadOnlyCollection<HomeworkCompletionDto>>(
                homeworkCompletions);
        }

        public async Task<HomeworkCompletionDto> GetHomeworkCompletion(int homeworkCompletionId)
        {
            var homeworkCompletion = await _unitOfWork.Homeworks.GetHomeworkCompletionById(homeworkCompletionId);
            var user = await _unitOfWork.Users.GetUser<PersonBase>(_jwtTokenReader.GetUserId());

            if (user.Role == Role.Student && user.Id != homeworkCompletion.StudentId)
            {
                throw new UnauthorizedAccessException("You dont have permission to watch homework completion.");
            }

            if (homeworkCompletion.Homework.Subject.GroupId != user.GroupId)
            {
                throw new UnauthorizedAccessException("You dont have permission to watch homework completion.");
            }

            return _mapper.Map<HomeworkCompletion, HomeworkCompletionDto>(homeworkCompletion);
        }

        public async Task DeleteHomeworkCompletion(int homeworkCompletionId)
        {
            var homeworkCompletion = await _unitOfWork.Homeworks.GetHomeworkCompletionById(homeworkCompletionId);
            var user = await _unitOfWork.Users.GetUser<PersonBase>(_jwtTokenReader.GetUserId());

            if (user.Role == Role.Student && user.Id != homeworkCompletion.StudentId)
            {
                throw new UnauthorizedAccessException("You dont have permission to remove homework completion.");
            }
            
            if (homeworkCompletion.Mark != null)
            {
                throw new ValidationException("You can't remove assessed homework completion.");
            }
            
            _unitOfWork.HomeworkAttachment
                .DeleteFileAttachments(homeworkCompletion.CompletionAttachments
                    .Select(file => file.FileAttachment)
                    .ToArray());

            await _unitOfWork.Homeworks.DeleteHomeworkCompletionById(homeworkCompletionId);
            await _unitOfWork.SaveChanges();
        }

        public async Task<HomeworkDto> AddHomeworkAttachment(int homeworkId, IFormFile file)
        {
            var homework = await _unitOfWork.Homeworks.GetHomeworkById(homeworkId, true);

            if (homework.Subject.TeacherId != _jwtTokenReader.GetUserId())
            {
                throw new UnauthorizedAccessException("You dont have permission to add new homework.");
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

        public async Task<HomeworkCompletionDto> AddHomeworkCompletion(int homeworkId,
            UploadHomeworkCompletionDto uploadHomeworkCompletionDto)
        {
            var homework = await _unitOfWork.Homeworks.GetHomeworkById(homeworkId, true);
            var user = await _unitOfWork.Users.GetUser<Student>(_jwtTokenReader.GetUserId());

            if (homework.Subject.GroupId != user.GroupId)
            {
                throw new UnauthorizedAccessException("You dont have permission to add new homework completion.");
            }

            var isExist = await _unitOfWork.Homeworks.IsExistHomeworkCompletion(homeworkId, user.Id);
            if (isExist)
            {
                throw new ValidationException("Homework completion is already exist.");
            }

            Task<FileAttachment>[] uploadTasks = uploadHomeworkCompletionDto.Files
                .Select(_fileService.CreateAttachment)
                .ToArray();

            var fileAttachments = await Task.WhenAll(uploadTasks);
            var homeworkAttachments = fileAttachments.Select(file => new CompletionAttachment()
            {
                FileAttachment = file
            });

            HomeworkCompletion homeworkCompletion = new HomeworkCompletion()
            {
                CompletionAttachments = homeworkAttachments.ToArray(),
                HomeworkId = homeworkId,
                StudentId = user.Id
            };

            await _unitOfWork.Homeworks.AddHomeworkCompletion(homeworkCompletion);
            await _unitOfWork.SaveChanges();

            return _mapper.Map<HomeworkCompletion, HomeworkCompletionDto>(homeworkCompletion);
        }

        public async Task<HomeworkCompletionDto> UpdateHomeworkCompletionMark(
            UpdateHomeworkCompletionMarkDto updateHomeworkDto, int homeworkCompletionId)
        {
            var homeworkCompletion = await _unitOfWork.Homeworks.GetHomeworkCompletionById(homeworkCompletionId);
            var user = await _unitOfWork.Users.GetUser<Teacher>(_jwtTokenReader.GetUserId());

            if (homeworkCompletion.Homework.Subject.GroupId != user.GroupId)
            {
                throw new UnauthorizedAccessException("You dont have permission to add new homework completion.");
            }

            homeworkCompletion.Mark = updateHomeworkDto.Mark;
            await _unitOfWork.SaveChanges();

            homeworkCompletion = await _unitOfWork.Homeworks.GetHomeworkCompletionById(homeworkCompletionId);
            return _mapper.Map<HomeworkCompletion, HomeworkCompletionDto>(homeworkCompletion);
        }

        public async Task<HomeworkCompletionDto> UpdateHomeworkCompletion(int homeworkCompletionId,
            UpdateHomeworkCompletionDto updateHomeworkCompletionDto)
        {
            var homeworkCompletion = await _unitOfWork.Homeworks.GetHomeworkCompletionById(homeworkCompletionId);
            var user = await _unitOfWork.Users.GetUser<Student>(_jwtTokenReader.GetUserId());
            var homework = await _unitOfWork.Homeworks.GetHomeworkById(homeworkCompletion.HomeworkId, true);

            if (homework.Subject.GroupId != user.GroupId)
            {
                throw new UnauthorizedAccessException("You dont have permission to add new homework!");
            }

            Task<FileAttachment>[] uploadTasks = updateHomeworkCompletionDto.Files
                .Select(_fileService.CreateAttachment)
                .ToArray();

            var fileAttachments = await Task.WhenAll(uploadTasks);
            _unitOfWork.HomeworkAttachment
                .DeleteFileAttachments(homeworkCompletion.CompletionAttachments
                    .Select(file => file.FileAttachment)
                    .ToArray());

            homeworkCompletion.CompletionAttachments = fileAttachments.Select(file => new CompletionAttachment()
            {
                FileAttachment = file
            }).ToList();

            await _unitOfWork.SaveChanges();

            return _mapper.Map<HomeworkCompletion, HomeworkCompletionDto>(homeworkCompletion);
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