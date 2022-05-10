using System;

namespace AchieveIt.API.Models
{
    public class RefreshTokenModel
    {
        public string AccessToken { get; set; }
        
        public Guid RefreshToken { get; set; }
    }
}