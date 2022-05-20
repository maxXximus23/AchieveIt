using System.Collections.Generic;

namespace AchieveIt.DataAccess.Entities
{
    public class Subject : EntityBase<int>
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int? GroupId { get; set; }
        
        public int? TeacherId { get; set; }
        
        public int? AssistTeacherId { get; set; }
        
        public Group Group { get; set; }
        
        public Teacher Teacher { get; set; }
        
        public Teacher AssistTeacher { get; set; }
        
        public string IconUrl { get; set; }

        public ICollection<Homework> Homeworks { get; set; }

        public Subject()
        {
            Homeworks = new List<Homework>();
        }
    }
}