using System.Threading.Tasks;
using AchieveIt.BusinessLogic.DTOs.Auth;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface IAuthService
    {
        public Task<AuthUserDto> RegisterStudent(RegisterStudentDto registerStudentDto);
        
        public Task<AuthUserDto> RegisterTeacher(RegisterTeacherDto registerTeacherDto);
        
        public Task<AuthUserDto> RegisterAdmin(RegisterAdminDto registerAdminDto);
    }
}