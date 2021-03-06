using System;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.Shared.Options;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AchieveIt.BusinessLogic.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobStorageOptions _blobStorageOptions;
        public BlobService(BlobServiceClient blobServiceClient, IOptions<BlobStorageOptions> blobStorageOptions)
        {
            _blobServiceClient = blobServiceClient;
            _blobStorageOptions = blobStorageOptions.Value;
        }

        public async Task<BlobFileInfo> GetBlobAsync(string name, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(name);
            var blobDownloadInfo = await blobClient.DownloadAsync();

            return new BlobFileInfo(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
        }

        public async Task<string> UploadFileBlobAsync(
            IFormFile file, string containerName, bool inline = false, string fileName = null)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            var newGuid = Guid.NewGuid().ToString();
            var blob = container.GetBlobClient(newGuid);
            var header = new BlobHttpHeaders()
            {
                ContentType = file.ContentType,
                ContentDisposition = $"{(inline ? "inline" : "attachment")}; filename=\"{fileName ?? file.FileName}\";"
            };

            await blob.UploadAsync(file.OpenReadStream(), header);
            return newGuid;
        }

        public string GenerateSaS(
            string containerName, 
            string filename, 
            DateTime expireDate, 
            BlobSasPermissions permission)
        {
            var blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = containerName,
                BlobName = filename,
                ExpiresOn = expireDate
            };
            
            blobSasBuilder.SetPermissions(permission);
            
            var sasUriBuilder = new UriBuilder
            {
                Scheme = Uri.UriSchemeHttps,
                Host = _blobStorageOptions.SasHost,
                Path = $"{containerName}/{filename}"
            };

            StorageSharedKeyCredential sharedKeyCredential =
                new StorageSharedKeyCredential(_blobStorageOptions.AccountName, _blobStorageOptions.AccountKey);

            sasUriBuilder.Query = blobSasBuilder.ToSasQueryParameters(sharedKeyCredential).ToString();
            return sasUriBuilder.ToString();
        }

        public Task UploadContentBlobAsync(string content, string fileName)
        {
            throw new System.NotImplementedException();
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("avatars");
            var blobClient = await containerClient.GetBlobClient(blobName).DeleteAsync();
        }
    }
}