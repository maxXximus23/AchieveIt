using AchieveIt.BusinessLogic.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.DTOs.Group;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.Repositories.Contracts;
using Microsoft.AspNetCore.Http;

namespace AchieveIt.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper, IGroupService groupService)
        {
            _mapper = mapper;
            _groupService = groupService;
            _userService = userService;
        }

        [HttpPost("Group/Student")]
        public async Task<IActionResult> AssignStudentToGroup([FromBody]AssignUserDto assignUserDto)
        {
            await _userService.AssignUserToGroup(assignUserDto.Email, assignUserDto.GroupId);
            return NoContent();
        }
        
        [HttpPost("Group/Teacher")]
        public async Task<IActionResult> AssignTeacherToGroup([FromBody]AssignUserDto assignUserDto)
        {
            await _userService.AssignTeacherToGroup(assignUserDto.Email, assignUserDto.GroupId);
            return NoContent();
        }
        
        [HttpDelete("Group/Student/{studentId}")]
        public async Task<IActionResult> DeleteStudentFromGroup([FromRoute]int studentId)
        {
            await _userService.DeleteStudentFromGroup(studentId);
            return NoContent();
        }
    }
}
