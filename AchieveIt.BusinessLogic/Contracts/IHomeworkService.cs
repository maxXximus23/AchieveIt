using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.DTOs.Homework;
using AchieveIt.BusinessLogic.DTOs.Homework.Completion;
using Microsoft.AspNetCore.Http;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface IHomeworkService
    {
        public Task<HomeworkDto> GetHomeworkById(int id);
        public Task DeleteHomeworkById(int id);

        public Task<IReadOnlyCollection<HomeworkCompletionDto>> GetHomeworkCompletions(int homeworkId);

        public Task<HomeworkCompletionDto> GetHomeworkCompletion(int homeworkCompletionId);

        public Task DeleteHomeworkCompletion(int homeworkCompletionId);
        
        public Task<HomeworkDto> AddHomeworkAttachment(int homeworkId, IFormFile file);

        public Task<HomeworkCompletionDto> AddHomeworkCompletion(int homeworkId,
            UploadHomeworkCompletionDto uploadHomeworkCompletionDto);
        
        public Task<HomeworkCompletionDto> UpdateHomeworkCompletionMark(
            UpdateHomeworkCompletionMarkDto updateHomeworkDto, int homeworkCompletionId);
        
        public Task<HomeworkCompletionDto> UpdateHomeworkCompletion(int homeworkId,
            UpdateHomeworkCompletionDto updateHomeworkCompletionDto);

        public Task<HomeworkDto> UpdateHomework(UpdateHomeworkDto updateHomeworkDto, int homeworkId);
    }
}