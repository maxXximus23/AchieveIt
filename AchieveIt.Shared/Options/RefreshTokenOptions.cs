using System;

namespace AchieveIt.Shared.Options
{
    public class RefreshTokenOptions
    {
        public const string RefreshTokenSectionName = "RefreshToken";
        
        public int ExpiresOnMonth { get; set; }
    }
}