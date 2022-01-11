using System;

namespace AchieveIt.DataAccess.Entities
{
    public class User : EntityBase<int>
    {
        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public string Patronymic { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public string Group { get; set; }
        
        public DateTime Birthday { get; set; }
    }
}