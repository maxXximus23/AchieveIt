using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.Repositories.Contracts;
using AchieveIt.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AchieveIt.DataAccess.Repositories
{
    public class HomeworkRepository : IHomeworkRepository
    {
        private readonly DatabaseContext _context;

        public HomeworkRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task CreateHomework(Homework homework)
        {
            await _context.Homeworks.AddAsync(homework);
        }

        public async Task<Homework> GetHomeworkById(int id, bool withTracking = false)
        {
            var query = withTracking
                ? _context.Homeworks.AsTracking() 
                : _context.Homeworks.AsNoTracking();
            
            return await query
                       .Where(homework => homework.Id == id)
                       .Include(homework => homework.Subject)
                       .Include(homework => homework.Attachments)
                       .ThenInclude(attachment => attachment.FileAttachment)
                       .FirstOrDefaultAsync(homework => homework.Id == id)
                   ?? throw new NotFoundException($"Homework with id {id} has not found.");
        }

        public async Task AddHomeworkCompletion(HomeworkCompletion homeworkCompletion)
        {
            await _context.HomeworkCompletions.AddAsync(homeworkCompletion);
        }

        public async Task<bool> IsExistHomeworkCompletion(int homeworkId, int studentId)
        {
            return await _context.HomeworkCompletions.AnyAsync(homeworkCompletion =>
                homeworkCompletion.StudentId == studentId
                && homeworkCompletion.HomeworkId == homeworkId);
        }

        public async Task<HomeworkCompletion> GetHomeworkCompletionById(int homeworkCompletionId)
        {
            return await _context.HomeworkCompletions
                       .Include(homeworkCompletion => homeworkCompletion.CompletionAttachments)
                       .ThenInclude(homeworkCompletion => homeworkCompletion.FileAttachment)
                       .Include(homeworkCompletion => homeworkCompletion.Homework)
                       .ThenInclude(homework => homework.Subject)
                       .FirstOrDefaultAsync(homeworkCompletion => homeworkCompletion.Id == homeworkCompletionId)
                   ?? throw new ValidationException($"Homework completion with id: {homeworkCompletionId} has not found.");
        }

        public async Task<IReadOnlyCollection<HomeworkCompletion>> GetHomeworkCompletions(int homeworkId)
        {
            return await _context.HomeworkCompletions
                       .Include(homeworkCompletion => homeworkCompletion.CompletionAttachments)
                       .ThenInclude(homework => homework.FileAttachment)
                       .Where(homeworkCompletion => homeworkCompletion.HomeworkId == homeworkId)
                       .ToArrayAsync() 
                   ?? throw new ValidationException($"Homework completions with homework id: {homeworkId} has not found.");
        }

        public async Task DeleteHomeworkCompletionById(int homeworkCompletionId)
        {
            var homeworkCompletion = await GetHomeworkCompletionById(homeworkCompletionId);
            _context.HomeworkCompletions.Remove(homeworkCompletion);
        }

        public async Task DeleteHomeworkById(int id)
        {
            Homework homework = await GetHomeworkById(id);
            _context.Homeworks.Remove(homework);
        }

        public void UpdateHomework(Homework homework)
        {
            _context.Homeworks.Update(homework);
        }

        public async Task<int> CountStudentHomeworks(int studentId)
        {
            return await _context.HomeworkCompletions.CountAsync(completion => completion.StudentId == studentId);
        }
    }
}