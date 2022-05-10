using System.Threading.Tasks;
using AchieveIt.BusinessLogic.DTOs.Group;

namespace AchieveIt.BusinessLogic.Contracts
{
    public interface IGroupService
    {
        public Task<GroupDto> CreateGroup(CreateGroupDto createGroupDto);

        public Task<GroupDto> UpdateGroup(int id, UpdateGroupDto updateGroupDto);

        public Task DeleteGroup(int id);
    }
}