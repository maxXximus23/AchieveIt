using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Subject;
using AchieveIt.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AchieveIt.API.Controllers
{
    
    [Authorize(Roles = "Admin")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<DetailedSubjectDto>> GetAllSubjects()
        {
            return await _subjectService.GetAllSubject();
        }
        
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