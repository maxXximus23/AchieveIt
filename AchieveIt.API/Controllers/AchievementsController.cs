using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Achievement;
using AchieveIt.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AchieveIt.API.Controllers
{
    public class AchievementsController : ControllerBase
    {
        private readonly IAchievementService _achievementService;

        public AchievementsController(IAchievementService achievementService)
        {
            _achievementService = achievementService;
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(AchievementDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAchievement([FromForm]CreateAchievementDto createAchievementDto)
        {
            return new ObjectResult(await _achievementService.CreateAchievement(createAchievementDto))
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost("students/{studentId}")]
        [ProducesResponseType(typeof(AchievementUserDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateStudentAchievement(
            [FromBody]CreateStudentAchievementDto createStudentAchievementDto, [FromRoute] int studentId)
        {
            return new ObjectResult(await _achievementService.CreateStudentAchievement(
                createStudentAchievementDto, studentId))
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("{achievementId}")]
        public async Task<AchievementDto> UpdateAchievement([FromForm] UpdateAchievementDto updateAchievementDto,
            [FromRoute]int achievementId)
        {
            return await _achievementService.UpdateAchievement(updateAchievementDto, achievementId);
        }
                
        [Authorize(Roles = "Admin")]
        [HttpDelete("{achievementId}")]
        public async Task<IActionResult> DeleteAchievement([FromRoute]int achievementId)
        {
            await _achievementService.DeleteAchievementById(achievementId);
            return NoContent();
        }
        
        [Authorize(Roles = "Admin")]
        [HttpDelete("students/{studentId}")]
        public async Task<IActionResult> DeleteStudentAchievement(
            [FromBody]DeleteStudentAchievementDto deleteStudentAchievementDto,
            [FromRoute]int studentId)
        {
            await _achievementService.DeleteStudentAchievementById(deleteStudentAchievementDto, studentId);
            return NoContent();
        }
        
        [HttpGet("{achievementId}")]
        public async Task<AchievementDto> GetAchievement([FromRoute]int achievementId)
        {
            return await _achievementService.GetAchievementById(achievementId);
        }
        
        [HttpGet("student/{studentId}")]
        public async Task<IReadOnlyCollection<AchievementDto>> GetStudentAchievements([FromRoute]int studentId)
        {
            return await _achievementService.GetStudentAchievements(studentId);
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<AchievementDto>> GetAchievements()
        {
            return await _achievementService.GetAchievements();
        }
    }
}