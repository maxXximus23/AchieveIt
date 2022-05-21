using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Homework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [HttpPut("{homeworkId}/attachments")]
        public async Task<HomeworkDto> AddHomeworkAttachment(
            [FromForm]UploadHomeworkAttachmentDto uploadHomeworkAttachmentDto, 
            [FromRoute]int homeworkId)
        {
            return await _homeworkService.AddHomeworkAttachment(homeworkId, uploadHomeworkAttachmentDto.File);
        }
    }
}