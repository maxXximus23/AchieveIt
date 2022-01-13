using System;

namespace AchieveIt.DataAccess.Entities
{
    public abstract class User : EntityBase<int>
    {
        public string Email { get; set; }
        
        public string Password { get; set; }

        public Role Role { get; set; }
    }
}