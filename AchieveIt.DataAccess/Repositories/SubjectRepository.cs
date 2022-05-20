using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.Repositories.Contracts;
using AchieveIt.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AchieveIt.DataAccess.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly DatabaseContext _context;

        public SubjectRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task CreateSubject(Subject subject)
        {
            await _context.Subjects.AddAsync(subject);
        }

        public async Task<IReadOnlyCollection<Subject>> GetAllSubject()
        {
            return await _context.Subjects
                .AsNoTracking()
                .Include(subject => subject.Group)
                .Include(subject => subject.Teacher)
                .Include(subject => subject.AssistTeacher)
                .ToArrayAsync();
        }

        public async Task<Subject> GetSubjectById(int id)
        {
            return await _context.Subjects
                       .Include(subject => subject.Group)
                       .Include(subject => subject.Teacher)
                       .Include(subject => subject.AssistTeacher)
                       .FirstOrDefaultAsync(subject => subject.Id == id)
                   ?? throw new NotFoundException($"Subject with id {id} has not found.");
        }

        public async Task<IReadOnlyCollection<Homework>> GetAllSubjectHomeworksById(int subjectId)
        {
            return await _context.Homeworks
                .AsNoTracking()
                .Where(subject => subject.Id == subjectId)
                .Include(subject => subject.Attachments)
                .ThenInclude(attachment => attachment.FileAttachment)
                .ToArrayAsync();
        }

        public void UpdateSubject(Subject subject)
        {
            _context.Subjects.Update(subject);
        }

        public async Task DeleteSubject(int id)
        {
            Subject subject = await GetSubjectById(id);
            _context.Subjects.Remove(subject);
        }
    }
}