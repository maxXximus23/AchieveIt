using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.DTOs.Subject;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface ISubjectService
    {
        public Task<SubjectDto> CreateSubject(CreateSubjectDto createSubjectDto);

        public Task<IReadOnlyCollection<DetailedSubjectDto>> GetAllSubject();

        public Task<DetailedSubjectDto> GetSubjectById(int id);

        public Task<SubjectDto> UpdateSubject(int id, UpdateSubjectDto updateSubjectDto);
        
        public Task DeleteSubject(int id);
    }
}