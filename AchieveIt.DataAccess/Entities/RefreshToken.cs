using System;

namespace AchieveIt.DataAccess.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        
        public Guid JwtId { get; set; }

        public DateTime ExpireDate { get; set; }
        
        public bool IsValidate { get; set; }
    }
}