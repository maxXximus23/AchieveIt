using System;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.DataAccess.UnitOfWork;
using AchieveIt.Shared.Extensions;
using AutoMapper;
using Kirpichyov.FriendlyJwt.Contracts;

namespace AchieveIt.BusinessLogic.Services
{
    public class HomeworkAttachmentService : IHomeworkAttachmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenReader _jwtTokenReader;
        private readonly IMapper _mapper;

        public HomeworkAttachmentService(IUnitOfWork unitOfWork, IMapper mapper, IJwtTokenReader jwtTokenReader)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtTokenReader = jwtTokenReader;
        }

        public async Task DeleteHomeworkAttachment(int homeworkAttachmentId)
        {
            var homeworkFileAttachment = await _unitOfWork.HomeworkAttachment
                .GetHomeworkAttachmentById(homeworkAttachmentId);
            
            if (homeworkFileAttachment.Homework.Subject.TeacherId != _jwtTokenReader.GetUserId())
            {
                throw new UnauthorizedAccessException("You dont have permission to delete homework attachment!");
            }
            
            _unitOfWork.HomeworkAttachment.DeleteFileAttachments(homeworkFileAttachment.FileAttachment);
            await _unitOfWork.SaveChanges();
        }
    }
}