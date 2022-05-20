using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;

namespace AchieveIt.DataAccess.Repositories.Contracts
{
    public interface IHomeworkRepository
    {
        public Task CreateHomework(Homework homework);

        public Task<Homework> GetHomeworkById(int id, bool withTracking = false);

        public Task DeleteHomeworkById(int id);
        
        public void UpdateHomework(Homework homework);
    }
}