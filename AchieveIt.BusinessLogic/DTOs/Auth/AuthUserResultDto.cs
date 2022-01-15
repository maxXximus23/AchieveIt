using System;

namespace AchieveIt.BusinessLogic.DTOs.Auth
{
    public class AuthUserResultDto
    {
        public bool IsSuccess { get; set; }
        
        public string Token { get; set; }
        public DateTime ExpiresOnUtc  { get; set; }
        
        public Guid RefreshToken { get; set; }
    }
}