using System;

namespace AchieveIt.BusinessLogic.DTOs.Auth
{
    public class RefreshTokenDto
    {
        public string AccessToken { get; set; }
        
        public Guid RefreshToken { get; set; }
    }
}