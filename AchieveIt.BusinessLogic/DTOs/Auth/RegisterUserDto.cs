namespace AchieveIt.BusinessLogic.DTOs.Auth
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        
        public RoleDto Role { get; set; }
    }
}