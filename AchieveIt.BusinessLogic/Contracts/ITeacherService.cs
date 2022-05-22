using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.DTOs.Homework;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface ITeacherService
    {
        public Task<HomeworkDto> CreateHomework(int subjectId, CreateHomeworkDto createHomeworkDto);

        public Task<IReadOnlyCollection<HomeworkDto>> GetAllHomework();

        public Task<HomeworkDto> GetHomeworkById(int id);
        
        public Task<HomeworkDto> UpdateHomework(int id, UpdateHomeworkDto updateHomeworkDto);
        
        public Task<HomeworkDto> DeleteHomework(int id);
    }
}