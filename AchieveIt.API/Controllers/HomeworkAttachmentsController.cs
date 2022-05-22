using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AchieveIt.API.Controllers
{
    public class HomeworkAttachmentsController : ControllerBase
    {
        private readonly IHomeworkAttachmentService _homeworkAttachmentService;

        public HomeworkAttachmentsController(IHomeworkAttachmentService homeworkAttachmentService)
        {
            _homeworkAttachmentService = homeworkAttachmentService;
        }
        
        [Authorize(Roles = "Teacher")]
        [HttpDelete("{homeworkAttachmentId}")]
        public async Task DeleteHomeworkAttachment([FromRoute]int homeworkAttachmentId)
        {
            await _homeworkAttachmentService.DeleteHomeworkAttachment(homeworkAttachmentId);
        }
    }
}