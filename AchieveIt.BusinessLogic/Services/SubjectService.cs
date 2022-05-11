using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Subject;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.UnitOfWork;
using AutoMapper;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Unidecode.NET;

namespace AchieveIt.BusinessLogic.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;

        public SubjectService(IUnitOfWork unitOfWork, IMapper mapper, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<SubjectDto> CreateSubject(CreateSubjectDto createSubjectDto)
        {
            Subject subject = _mapper.Map<CreateSubjectDto, Subject>(createSubjectDto);

            subject.IconUrl = await UploadFile(createSubjectDto.Icon);
            await _unitOfWork.Subjects.CreateSubject(subject);
            await _unitOfWork.SaveChanges();
            
            return _mapper.Map<Subject, SubjectDto>(subject);
        }

        public async Task<IReadOnlyCollection<DetailedSubjectDto>> GetAllSubject()
        {
            var subjects = await _unitOfWork.Subjects.GetAllSubject();
            return _mapper.Map<IReadOnlyCollection<Subject>, IReadOnlyCollection<DetailedSubjectDto>>(subjects);
        }

        public async Task<DetailedSubjectDto> GetSubjectById(int id)
        {
            Subject subject = await _unitOfWork.Subjects.GetSubjectById(id);
            return _mapper.Map<Subject, DetailedSubjectDto>(subject);
        }

        public async Task<SubjectDto> UpdateSubject(int id, UpdateSubjectDto updateSubjectDto)
        {
            Subject subject = await _unitOfWork.Subjects.GetSubjectById(id);

            _mapper.Map(updateSubjectDto, subject);
            if (updateSubjectDto.Icon is not null)
            {
                subject.IconUrl = await UploadFile(updateSubjectDto.Icon);
            }

            _unitOfWork.Subjects.UpdateSubject(subject);
            await _unitOfWork.SaveChanges();

            return _mapper.Map<Subject, SubjectDto>(subject);
        }

        public async Task DeleteSubject(int id)
        {
            await _unitOfWork.Subjects.DeleteSubject(id);
            await _unitOfWork.SaveChanges();
        }
        
        private async Task<string> UploadFile(IFormFile avatar)
        {
            string fileName = avatar.FileName.Unidecode();
            string blobName = await _blobService.UploadFileBlobAsync(avatar, "avatars", true, fileName);
            string avatarUrl = _blobService.GenerateSaS("avatars", blobName, DateTime.MaxValue, BlobSasPermissions.Read);

            return avatarUrl;
        }
    }
}