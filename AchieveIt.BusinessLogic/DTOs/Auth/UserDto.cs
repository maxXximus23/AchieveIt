namespace AchieveIt.BusinessLogic.DTOs.Auth
{
    public abstract class UserDto
    {
        public string Email { get; set; }
        
        public string Password { get; set; }

        public RoleDto Role { get; set; }
    }
}