using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.DataAccess.Entities;

namespace AchieveIt.DataAccess.Repositories.Contracts
{
    public interface IGroupRepository
    {
        public Task CreateGroup(Group group);

        public void UpdateGroup(Group group);
        
        public Task DeleteGroup(int id);

        public Task<Group> GetGroup(int id);

        public Task<List<Group>> GetAllGroups();

        public Task<Group> GetGroupById(int id);

        public Task<bool> IsGroupExist(int id);
    }
}