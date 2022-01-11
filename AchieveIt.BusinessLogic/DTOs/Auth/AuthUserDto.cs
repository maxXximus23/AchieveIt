namespace AchieveIt.BusinessLogic.DTOs.Auth
{
    public class AuthUserDto
    {
        public bool IsSuccess { get; set; }
        
        public string Token { get; set; }
        
        public string RefreshToken { get; set; }
    }
}