﻿namespace AchieveIt.DataAccess.Entities
{
    public class TeacherGroup : EntityBase<int>
    {
        public int TeacherId { get; set; }
        
        public int GroupId { get; set; }
        
        public Teacher Teacher { get; set; }
    }
}