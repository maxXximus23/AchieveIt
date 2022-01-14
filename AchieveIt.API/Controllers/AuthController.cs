using System.Threading.Tasks;
using AchieveIt.API.Model;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Auth;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AchieveIt.API.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("Students")]
        public async Task<AuthUserResultDto> RegisterStudent([FromBody] RegisterStudentModel registerStudentModel)
        {
            var studentDto = _mapper.Map<RegisterStudentModel, RegisterStudentDto>(registerStudentModel);
            
            return await _authService.RegisterStudent(studentDto);
        }
        
        [HttpPost("Teachers")] 
        public async Task<AuthUserResultDto> RegisterStudent([FromBody] RegisterTeacherModel registerTeacherModel)
        {
            var teacherDto = _mapper.Map<RegisterTeacherModel, RegisterTeacherDto>(registerTeacherModel);
            
            return await _authService.RegisterTeacher(teacherDto);
        }
        
        [HttpPost("Admins")] 
        public async Task<AuthUserResultDto> RegisterAdmin([FromBody] RegisterAdminModel registerAdminModel)
        {
            var adminDto = _mapper.Map<RegisterAdminModel, RegisterAdminDto>(registerAdminModel);
            
            return await _authService.RegisterAdmin(adminDto);
        }
    }
}