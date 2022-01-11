using System.Threading.Tasks;
using AchieveIt.BusinessLogic.DTOs.Auth;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface IAuthService
    {
        public Task<AuthUserDto> RegisterStudent(RegisterStudentDto registerStudentDto);
        
        public Task<AuthUserDto> RegisterTeacher();
        
        public Task<AuthUserDto> RegisterAdmin();
    }
}