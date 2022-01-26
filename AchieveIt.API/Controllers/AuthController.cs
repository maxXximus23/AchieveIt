using System.Threading.Tasks;
using AchieveIt.API.Models;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Auth;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AchieveIt.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper, IBlobService blobService)
        {
            _authService = authService;
            _mapper = mapper;
            _blobService = blobService;
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
        
        [HttpPost("Refresh")] 
        [AllowAnonymous]
        public async Task<AuthUserResultDto> RefreshToken([FromBody] RefreshTokenModel refreshToken)
        {
            var refreshTokenDto = _mapper.Map<RefreshTokenModel, RefreshTokenDto>(refreshToken);
            
            return await _authService.RefreshToken(refreshTokenDto);
        }

        [HttpPost] 
        [AllowAnonymous]
        public async Task<AuthUserResultDto> SignIn([FromBody] SignInModel signInModel)
        {
            var signInDto = _mapper.Map<SignInModel, SignInDto>(signInModel);
            
            return await _authService.SignIn(signInDto);
        }
    }
}