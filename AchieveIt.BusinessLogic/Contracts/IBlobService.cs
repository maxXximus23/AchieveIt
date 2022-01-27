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

        public Task UploadFileBlobAsync(IFormFile file, string containerName, bool inline = false);

        public string GenerateSaS(
            string containerName, 
            string filename, 
            DateTime expireDate, 
            BlobSasPermissions permission);

        public Task UploadContentBlobAsync(string content, string fileName);

        public Task DeleteBlobAsync(string blobName);
    }
}