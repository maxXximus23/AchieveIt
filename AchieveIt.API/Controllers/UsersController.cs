using System.Threading.Tasks;
using AchieveIt.API.Model;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AchieveIt.API.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UsersController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Students")]
        public async Task<AuthUserDto> RegisterStudent([FromBody] RegisterStudentModel registerStudentModel)
        {
            var registerStudentDto = new RegisterStudentDto()
            {
                Name = registerStudentModel.Name,
                Birthday = registerStudentModel.Birthday,
                Email = registerStudentModel.Email,
                Group = registerStudentModel.Group,
                Password = registerStudentModel.Password,
                Patronymic = registerStudentModel.Patronymic,
                Surname = registerStudentModel.Surname
            };
            
            return await _authService.RegisterStudent(registerStudentDto);
        }
    }
}