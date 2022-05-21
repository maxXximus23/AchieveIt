using System;

namespace AchieveIt.DataAccess.Entities
{
    public class FileAttachment : EntityBase<int>
    {
        public string OriginalName { get; set; }
        
        public string Url { get; set; }
        
        public DateTime UploadTime { get; set; }
    }
}