using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchieveIt.DataAccess.Entities
{
    public class Group : EntityBase<int>
    {
        public string Name { get; set; }

        public string Faculty { get; set; }

        public string Department { get; set; }
        
        public string AvatarUrl { get; set; }
        
        public ICollection<TeacherGroup> TeacherGroups { get; set; }

        public Group()
        {
            TeacherGroups = new List<TeacherGroup>();
        }
    }
}
