using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.DTOs.Group;
using AchieveIt.DataAccess.Entities;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface IUserService
    {
        public Task<List<GroupDto>> GetAllGroups();

        public Task AssignUserToGroup(string email, int groupId);
        
        public Task AssignTeacherToGroup(string email, int groupId);

        public Task DeleteStudentFromGroup(int studentId);
    }
}
