using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.Repositories.Contracts;
using AchieveIt.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AchieveIt.DataAccess.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly DatabaseContext _context;

        public GroupRepository(DatabaseContext context)
        {
            _context = context;
        }
        
        public async Task CreateGroup(Group group)
        {
            await _context.Groups.AddAsync(group);
        }
        
        public async Task<Group> GetGroup(int id)
        {
            return await _context.Groups.FirstOrDefaultAsync(group => group.Id == id)
                   ?? throw new NotFoundException($"Group with id {id} has not found.");
        }
        
        public async Task<List<Group>> GetAllGroups()
        {
            return await _context.Groups.ToListAsync();
        }
        
        public async Task DeleteGroup(int id)
        {
            Group group = await GetGroup(id);
            _context.Groups.Remove(group);
        }

        public void UpdateGroup(Group group)
        {
            _context.Groups.Update(group);
        }

        public async Task<Group> GetGroupById(int id)
        {
            return await _context.Groups
                       .Include(group => group.TeacherGroups)
                       .FirstOrDefaultAsync(group => group.Id == id)
                   ?? throw new NotFoundException($"Group with id {id} has not found.");
        }

        public async Task<bool> IsGroupExist(int id)
        {
            return await _context.Groups.AnyAsync(group => group.Id == id);
        }
    }
}