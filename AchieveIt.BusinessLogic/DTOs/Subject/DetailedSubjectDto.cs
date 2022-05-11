using AchieveIt.BusinessLogic.DTOs.Group;
using AchieveIt.BusinessLogic.DTOs.User.Teacher;

namespace AchieveIt.BusinessLogic.DTOs.Subject
{
    public class DetailedSubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public GroupDto Group { get; set; }
        public SubjectTeacherDto AssistTeacher { get; set; }
        public SubjectTeacherDto Teacher { get; set; }
    }
}