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

        public async Task DeleteHomeworkById(int id)
        {
            Homework homework = await GetHomeworkById(id);
            _context.Homeworks.Remove(homework);
        }

        public void UpdateHomework(Homework homework)
        {
            _context.Homeworks.Update(homework);
        }
    }
}