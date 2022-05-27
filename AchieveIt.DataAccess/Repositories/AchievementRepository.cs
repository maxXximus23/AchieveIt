using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.Repositories.Contracts;
using AchieveIt.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AchieveIt.DataAccess.Repositories
{
    public class AchievementRepository : IAchievementRepository
    {
        private readonly DatabaseContext _context;

        public AchievementRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task CreateStudentAchievement(AchievementUser achievementUser)
        {
            await _context.AchievementUsers.AddAsync(achievementUser);
        }
        
        public async Task<bool> IsStudentHaveAchievement(int studentId, int achievementId)
        {
            return await _context.AchievementUsers.AnyAsync(user => user.UserId == studentId
            && user.AchievementId == achievementId);
        }

        public void UpdateAchievement(Achievement achievement)
        {
            _context.Achievements.Update(achievement);
        }

        public async Task DeleteAchievementById(int achievementId)
        {
            var achievement = await GetAchievementById(achievementId);
            _context.Achievements.Remove(achievement);
        }

        public async Task DeleteStudentAchievement(int achievementId, int studentId)
        {
            var achievementUser = await GetStudentAchievement(achievementId, studentId);
            _context.AchievementUsers.Remove(achievementUser);
        }

        public async Task<bool> IsAchievementExist(string achievementName)
        {
            return await _context.Achievements.AnyAsync(achievement => achievement.Name == achievementName);
        }

        public async Task<IReadOnlyCollection<Achievement>> GetAchievements()
        {
            return await _context.Achievements.ToArrayAsync();
        }

        public async Task<Achievement> GetAchievementByName(string achievementName)
        {
            return await _context.Achievements.FirstOrDefaultAsync(achievement => achievement.Name == achievementName);
        }

        public async Task<Achievement> GetAchievementById(int achievementId)
        {
            return await _context.Achievements.FirstOrDefaultAsync(achievement => achievement.Id == achievementId)
                ?? throw new ValidationException($"Achievement with id: {achievementId} has not found.");
        }
        
        public async Task<AchievementUser> GetStudentAchievement(int achievementId, int studentId)
        {
            return await _context.AchievementUsers.FirstOrDefaultAsync(achievement => achievement.UserId == studentId
            && achievement.AchievementId == achievementId);
        }
        
        public async Task<IReadOnlyCollection<Achievement>> GetStudentAchievements(int studentId)
        {
            return await _context.AchievementUsers
                .Where(achievementsUser => achievementsUser.UserId == studentId)
                .Select(achievementUser => achievementUser.Achievement)
                .ToArrayAsync();
        }

        public async Task CreateAchievement(Achievement achievement)
        {
            await _context.Achievements.AddAsync(achievement);
        }
    }
}