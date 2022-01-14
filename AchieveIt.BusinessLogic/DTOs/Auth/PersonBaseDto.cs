using System;

namespace AchieveIt.BusinessLogic.DTOs.Auth
{
    public abstract class PersonBaseDto : RegisterUserDto
    {
        public string Name { get; set; }

        public string Surname { get; set; }
        
        public string Patronymic { get; set; }
        
        public DateTime Birthday { get; set; }
    }
}