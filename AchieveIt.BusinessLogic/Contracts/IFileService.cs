using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface IFileService
    {
        public Task<string> UploadFile(IFormFile file);

        public Task<string> UploadAvatar(IFormFile file);

        public Task<FileAttachment> CreateAttachment(IFormFile homeworkAttachment);
    }
}