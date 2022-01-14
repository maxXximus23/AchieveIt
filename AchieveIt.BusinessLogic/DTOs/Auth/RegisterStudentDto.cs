using System;
using AchieveIt.DataAccess.Entities;

namespace AchieveIt.BusinessLogic.DTOs.Auth
{
    public class RegisterStudentDto : PersonBaseDto
    {
        public string Group { get; set; }
    }
}