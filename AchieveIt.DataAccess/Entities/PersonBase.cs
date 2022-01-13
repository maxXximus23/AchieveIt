using System;

namespace AchieveIt.DataAccess.Entities
{
    public abstract class PersonBase : User
    {
        public string Name { get; set; }
                
        public string Surname { get; set; }
                
        public string Patronymic { get; set; }
                
        public DateTime Birthday { get; set; }
    }
}