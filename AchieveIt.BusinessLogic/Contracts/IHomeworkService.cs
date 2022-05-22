using System.Threading.Tasks;
using AchieveIt.BusinessLogic.DTOs.Homework;
using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface IHomeworkService
    {
        public Task<HomeworkDto> GetHomeworkById(int id);
        public Task DeleteHomeworkById(int id);

        public Task<HomeworkDto> AddHomeworkAttachment(int homeworkId, IFormFile file);

        public Task<HomeworkDto> UpdateHomework(UpdateHomeworkDto updateHomeworkDto, int homeworkId);
    }
}