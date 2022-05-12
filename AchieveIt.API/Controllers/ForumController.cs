using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Forum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AchieveIt.API.Controllers
{
    [Authorize(Roles = "Student, Teacher")]
    public class ForumController : ControllerBase
    {
        private readonly IForumService _forumService;

        public ForumController(IForumService forumService)
        {
            _forumService = forumService;
        }

        [HttpPost("topics")]
        [ProducesResponseType(typeof(TopicDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTopicDto dto)
        {
            var topic = await _forumService.CreateTopic(dto);
            
            return new ObjectResult(topic)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }

        [HttpGet("topics")]
        public async Task<IReadOnlyCollection<TopicDto>> GetAll()
        {
            return await _forumService.GetTopics();
        }

        [HttpGet("topics/{id}")]
        public async Task<TopicDto> Get([FromRoute] int id)
        {
            return await _forumService.GetTopic(id);
        }

        [HttpDelete("topics/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _forumService.DeleteTopic(id);
            return NoContent();
        }

        [HttpPost("topics/{id}/comments")]
        [ProducesResponseType(typeof(TopicCommentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddComment([FromRoute] int id, [FromBody] AddCommentDto dto)
        {
            var comment = await _forumService.AddComment(id, dto);
            
            return new ObjectResult(comment)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
        
        [HttpGet("topics/{id}/comments")]
        public async Task<IReadOnlyCollection<TopicCommentDto>> GetAllComments([FromRoute] int id)
        {
            return await _forumService.GetComments(id);
        }

        [HttpGet("topics/comments/{id}")]
        public async Task<TopicCommentDto> GetComment([FromRoute] int id)
        {
            return await _forumService.GetComment(id);
        }
        
        [HttpPut("topics/comments/{id}")]
        [ProducesResponseType(typeof(TopicCommentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<TopicCommentDto> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDto dto)
        {
            return await _forumService.UpdateComment(id, dto);
        }
        
        [HttpDelete("topics/comments/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            await _forumService.DeleteComment(id);
            return NoContent();
        }
    }
}