namespace AchieveIt.BusinessLogic.DTOs.Subject
{
    public class SubjectDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int GroupId { get; set; }
        
        public int TeacherId { get; set; }
        
        public int AssistTeacherId { get; set; }
        
        public string IconUrl { get; set; }
    }
}