using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Homework;
using AchieveIt.BusinessLogic.DTOs.Subject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AchieveIt.API.Controllers
{
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly ITeacherService _teacherService;

        public SubjectController(ISubjectService subjectService, ITeacherService teacherService)
        {
            _subjectService = subjectService;
            _teacherService = teacherService;
        }
        
        [Authorize(Roles = "Teacher")]
        [HttpPost("{subjectId}/homeworks")]
        [ProducesResponseType(typeof(HomeworkDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateHomework([FromForm]CreateHomeworkDto createHomeworkDto,
            [FromRoute]int subjectId)
        {
            return new ObjectResult(await _teacherService.CreateHomework(subjectId, createHomeworkDto))
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
        
        [Authorize(Roles = "Teacher")]
        [HttpGet("{subjectId}/homeworks")]
        public async Task<IReadOnlyCollection<HomeworkDto>> GetSubjectHomeworksById([FromRoute]int subjectId)
        {
            return await _subjectService.GetAllSubjectHomeworksById(subjectId);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IReadOnlyCollection<DetailedSubjectDto>> GetAllSubjects()
        {
            return await _subjectService.GetAllSubject();
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("{subjectId}")]
        public async Task<DetailedSubjectDto> GetSubjectById(int subjectId)
        {
            return await _subjectService.GetSubjectById(subjectId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(SubjectDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateSubject([FromForm]CreateSubjectDto createSubjectDto)
        {
            return new ObjectResult(await _subjectService.CreateSubject(createSubjectDto))
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("{subjectId}")]
        public async Task<IActionResult> UpdateSubject([FromForm]UpdateSubjectDto updateSubjectDto, 
            [FromRoute]int subjectId)
        {
            await _subjectService.UpdateSubject(subjectId, updateSubjectDto);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{subjectId}")]
        public async Task<IActionResult> DeleteSubject([FromRoute] int subjectId)
        {
            await _subjectService.DeleteSubject(subjectId);
            return NoContent();
        }
    }
}