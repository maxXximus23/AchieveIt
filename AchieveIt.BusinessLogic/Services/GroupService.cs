using System;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Group;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.UnitOfWork;
using AutoMapper;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Unidecode.NET;

namespace AchieveIt.BusinessLogic.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;

        public GroupService(IUnitOfWork unitOfWork, IMapper mapper, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<GroupDto> GetGroupById(int id)
        {
            Group group = await _unitOfWork.Groups.GetGroupById(id);
            return _mapper.Map<Group, GroupDto>(group);
        }

        public async Task<GroupDto> UpdateGroup(int id, UpdateGroupDto updateGroupDto)
        {
            Group group = await _unitOfWork.Groups.GetGroupById(id);

            _mapper.Map(updateGroupDto, group);
            if (updateGroupDto.Avatar is not null)
            {
                group.AvatarUrl = await UploadFile(updateGroupDto.Avatar);
            }

            _unitOfWork.Groups.UpdateGroup(group);
            await _unitOfWork.SaveChanges();

            return _mapper.Map<Group, GroupDto>(group);
        }

        public async Task DeleteGroup(int id)
        {
            await _unitOfWork.Groups.DeleteGroup(id);
            await _unitOfWork.SaveChanges();
        }

        public async Task<GroupDto> CreateGroup(CreateGroupDto createGroupDto)
        {
            Group group = _mapper.Map<CreateGroupDto, Group>(createGroupDto);

            group.AvatarUrl = await UploadFile(createGroupDto.Avatar);
            await _unitOfWork.Groups.CreateGroup(group);
            await _unitOfWork.SaveChanges();
            
            return _mapper.Map<Group, GroupDto>(group);
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