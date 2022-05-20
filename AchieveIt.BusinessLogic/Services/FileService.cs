using System;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.Shared.Constants;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Unidecode.NET;

namespace AchieveIt.BusinessLogic.Services
{
    public class FileService : IFileService
    {
        private readonly IBlobService _blobService;
        
        public FileService(IBlobService blobService)
        {
            _blobService = blobService;
        }
        
        public async Task<string> UploadAvatar(IFormFile file)
        {
            return await UploadFile(file, FileConstants.Avatar, true);
        }
        
        public async Task<string> UploadFile(IFormFile file)
        {
            return await UploadFile(file, FileConstants.File);
        }

        private async Task<string> UploadFile(IFormFile file, string containerName, bool inline = false)
        {
            string fileName = file.FileName.Unidecode();
            string blobName = await _blobService.UploadFileBlobAsync(file, containerName, inline, fileName);
            string fileUrl =
                _blobService.GenerateSaS(containerName, blobName, DateTime.MaxValue, BlobSasPermissions.Read);

            return fileUrl;
        }
    }
}