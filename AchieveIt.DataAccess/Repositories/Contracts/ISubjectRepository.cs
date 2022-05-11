using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;

namespace AchieveIt.DataAccess.Repositories.Contracts
{
    public interface ISubjectRepository
    {
        public Task CreateSubject(Subject subject);
        
        public Task<IReadOnlyCollection<Subject>> GetAllSubject();
        
        public Task<Subject> GetSubjectById(int id);
        
        public void UpdateSubject(Subject subject);
        
        public Task DeleteSubject(int id);
    }
}