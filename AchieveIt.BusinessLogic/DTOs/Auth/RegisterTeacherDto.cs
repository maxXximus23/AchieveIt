using System;

namespace AchieveIt.BusinessLogic.DTOs.Auth
{
    public class RegisterTeacherDto
    {
        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public string Patronymic { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }

        public RoleDto Role { get; set; }
        
        public DateTime Birthday { get; set; }
    }
}