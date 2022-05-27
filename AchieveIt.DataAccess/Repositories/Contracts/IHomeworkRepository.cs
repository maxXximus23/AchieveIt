using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace AchieveIt.DataAccess.Repositories.Contracts
{
    public interface IHomeworkRepository
    {
        public Task CreateHomework(Homework homework);

        public Task<Homework> GetHomeworkById(int id, bool withTracking = false);

        public Task AddHomeworkCompletion(HomeworkCompletion homeworkCompletion);

        public Task<bool> IsExistHomeworkCompletion(int homeworkId, int studentId);

        public Task<HomeworkCompletion> GetHomeworkCompletionById(int homeworkId);

        public Task<IReadOnlyCollection<HomeworkCompletion>> GetHomeworkCompletions(int homeworkId);

        public Task DeleteHomeworkCompletionById(int homeworkCompletionId);
            
        public Task DeleteHomeworkById(int id);

        public void UpdateHomework(Homework homework);

        public Task<int> CountStudentHomeworks(int studentId);
    }
}