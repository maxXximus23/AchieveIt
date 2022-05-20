using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface IFileService
    {
        public Task<string> UploadFile(IFormFile file);

        public Task<string> UploadAvatar(IFormFile file);
    }
}