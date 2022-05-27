using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace AchieveIt.DataAccess.Entities
{
    public class Achievement : EntityBase<int>
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string Url { get; set; }
        
        public bool IsAuto { get; set; }
    }
}