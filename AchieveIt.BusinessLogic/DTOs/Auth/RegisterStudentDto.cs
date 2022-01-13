﻿using System;
using AchieveIt.DataAccess.Entities;

namespace AchieveIt.BusinessLogic.DTOs.Auth
{
    public class RegisterStudentDto
    {
        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public string Patronymic { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public string Group { get; set; }
        
        public RoleDto Role { get; set; }
        
        public DateTime Birthday { get; set; }
    }
}