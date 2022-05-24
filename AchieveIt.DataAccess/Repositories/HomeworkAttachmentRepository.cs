using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.Repositories.Contracts;
using AchieveIt.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AchieveIt.DataAccess.Repositories
{
    public class HomeworkAttachmentRepository : IHomeworkAttachmentRepository
    {
        private readonly DatabaseContext _context;

        public HomeworkAttachmentRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<HomeworkFileAttachment> GetHomeworkAttachmentById(int homeworkAttachmentId)
        {
            return await _context.HomeworkFileAttachments
                       .Include(fileAttachment => fileAttachment.FileAttachment)
                       .Include(homework => homework.Homework)
                       .ThenInclude(homework => homework.Subject)
                       .FirstOrDefaultAsync(homeworkAttachment => 
                       homeworkAttachment.Id == homeworkAttachmentId)
                   ?? throw new NotFoundException("Homework file attachment with id: " +
                                                  $"{homeworkAttachmentId} has not found.");
        }

        public void DeleteFileAttachments(params FileAttachment[] fileAttachment)
        {
            _context.FileAttachments.RemoveRange(fileAttachment);
        }
    }
}