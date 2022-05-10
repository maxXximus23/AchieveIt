using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Services;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface IBlobService
    {
        public Task<BlobFileInfo> GetBlobAsync(string name, string containerName);

        public Task<string> UploadFileBlobAsync(IFormFile file, string containerName, 
            bool inline = false, string fileName = null);

        public string GenerateSaS(
            string containerName, 
            string filename, 
            DateTime expireDate, 
            BlobSasPermissions permission);

        public Task UploadContentBlobAsync(string content, string fileName);

        public Task DeleteBlobAsync(string blobName);
    }
}