using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Homework;
using AchieveIt.BusinessLogic.DTOs.Homework.Completion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AchieveIt.API.Controllers
{
    public class HomeworksController : ControllerBase
    {
        private readonly IHomeworkService _homeworkService;

        public HomeworksController(IHomeworkService homeworkService)
        {
            _homeworkService = homeworkService;
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("{homeworkId}")]
        public async Task<HomeworkDto> GetHomeworksById([FromRoute]int homeworkId)
        {
            return await _homeworkService.GetHomeworkById(homeworkId);
        }
        
        [Authorize(Roles = "Teacher")]
        [HttpGet("{homeworkId}/completions")]
        public async Task<IReadOnlyCollection<HomeworkCompletionDto>> GetHomeworkCompletions(
            [FromRoute]int homeworkId)
        {
            return await _homeworkService.GetHomeworkCompletions(homeworkId);
        }
        
        [Authorize(Roles = "Student, Teacher")]
        [HttpGet("completions/{homeworkCompletionId}")]
        public async Task<HomeworkCompletionDto> GetHomeworkCompletion([FromRoute]int homeworkCompletionId)
        {
            return await _homeworkService.GetHomeworkCompletion(homeworkCompletionId);
        }
        
        [Authorize(Roles = "Student")]
        [HttpDelete("completions/{homeworkCompletionId}")]
        public async Task<IActionResult> DeleteHomeworkCompletion([FromRoute]int homeworkCompletionId)
        {
            await _homeworkService.DeleteHomeworkCompletion(homeworkCompletionId);
            return NoContent();
        }
        
        [Authorize(Roles = "Teacher")]
        [HttpDelete("{homeworkId}")]
        public async Task<IActionResult> DeleteHomeworkById([FromRoute]int homeworkId)
        {
            await _homeworkService.DeleteHomeworkById(homeworkId);
            return NoContent();
        }
        
        [Authorize(Roles = "Teacher")]
        [HttpPut("{homeworkId}")]
        public async Task<HomeworkDto> UpdateHomework([FromBody]UpdateHomeworkDto updateHomeworkDto,
            [FromRoute]int homeworkId)
        {
            return await _homeworkService.UpdateHomework(updateHomeworkDto, homeworkId);
        }
        
        [Authorize(Roles = "Teacher")]
        [HttpPut("completions/{homeworkCompletionId}/assess")]
        public async Task<HomeworkCompletionDto> UpdateHomeworkCompletionMark([FromBody]UpdateHomeworkCompletionMarkDto 
                updateHomeworkDto, [FromRoute]int homeworkCompletionId)
        {
            return await _homeworkService.UpdateHomeworkCompletionMark(updateHomeworkDto, homeworkCompletionId);
        }
        
        [Authorize(Roles = "Teacher")]
        [HttpPut("{homeworkId}/attachments")]
        public async Task<HomeworkDto> AddHomeworkAttachment(
            [FromForm]UploadHomeworkAttachmentDto uploadHomeworkAttachmentDto, 
            [FromRoute]int homeworkId)
        {
            return await _homeworkService.AddHomeworkAttachment(homeworkId, uploadHomeworkAttachmentDto.File);
        }
        
        [Authorize(Roles = "Student")]
        [HttpPost("{homeworkId}/completions")]
        public async Task<HomeworkCompletionDto> AddHomeworkCompletion(
            [FromForm]UploadHomeworkCompletionDto uploadHomeworkCompletionDto, 
            [FromRoute]int homeworkId)
        {
            return await _homeworkService.AddHomeworkCompletion(homeworkId, uploadHomeworkCompletionDto);
        }
        
        [Authorize(Roles = "Student")]
        [HttpPut("completions/{homeworkCompletionId}")]
        public async Task<HomeworkCompletionDto> UpdateHomeworkCompletion(
            [FromForm]UpdateHomeworkCompletionDto updateHomeworkCompletionDto, 
            [FromRoute]int homeworkCompletionId)
        {
            return await _homeworkService.UpdateHomeworkCompletion(homeworkCompletionId, updateHomeworkCompletionDto);
        }
    }
}