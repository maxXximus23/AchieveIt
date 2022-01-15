using System.Threading.Tasks;
using AchieveIt.BusinessLogic.DTOs.Auth;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface IAuthService
    {
        public Task<AuthUserResultDto> RegisterStudent(RegisterStudentDto registerStudentDto);
        
        public Task<AuthUserResultDto> RegisterTeacher(RegisterTeacherDto registerTeacherDto);
        
        public Task<AuthUserResultDto> RegisterAdmin(RegisterAdminDto registerAdminDto);

        public Task<AuthUserResultDto> RefreshToken(RefreshTokenDto refreshTokenDto);
    }
}