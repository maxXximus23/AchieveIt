using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Group;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AchieveIt.API.Controllers
{
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public GroupController(IGroupService groupService, IMapper mapper, IUserService userService)
        {
            _groupService = groupService;
            _mapper = mapper;
            _userService = userService;
        }
        
        [HttpGet]
        public async Task<List<GroupDto>> GetAllGroups()
        {
            return await _userService.GetAllGroups();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateGroup([FromForm]CreateGroupDto createGroupDto)
        {
            return new ObjectResult(await _groupService.CreateGroup(createGroupDto))
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("{groupId}")]
        public async Task<IActionResult> UpdateGroup([FromForm]UpdateGroupDto updateGroupDto, [FromRoute]int groupId)
        {
            await _groupService.UpdateGroup(groupId, updateGroupDto);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteGroup([FromRoute] int groupId)
        {
            await _groupService.DeleteGroup(groupId);
            return NoContent();
        }
    }
}