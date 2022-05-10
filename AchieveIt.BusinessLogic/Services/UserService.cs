using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.UnitOfWork;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.DTOs.Group;
using AchieveIt.Shared.Exceptions;

namespace AchieveIt.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<GroupDto>> GetAllGroups()
        {
            var groups = await _unitOfWork.Groups.GetAllGroups();
            return _mapper.Map<List<Group>, List<GroupDto>>(groups);
        }

        public async Task DeleteStudentFromGroup(int studentId)
        {
           Student student = await _unitOfWork.Users.GetUser<Student>(studentId);

           student.GroupId = null;
           _unitOfWork.Users.UpdateUser(student);
           await _unitOfWork.SaveChanges();
        }

        public async Task AssignUserToGroup(string email, int groupId)
        {
            Student student = await _unitOfWork.Users.GetUserByEmail<Student>(email);
            if (!await _unitOfWork.Groups.IsGroupExist(groupId))
            {
                throw new NotFoundException($"Group with id {groupId} has not found.");
            }

            student.GroupId = groupId;
            _unitOfWork.Users.UpdateUser(student);
            await _unitOfWork.SaveChanges();
        }
        
        public async Task AssignTeacherToGroup(string email, int groupId)
        {
            Group group = await _unitOfWork.Groups.GetGroupById(groupId);
            Teacher teacher = await _unitOfWork.Users.GetUserByEmail<Teacher>(email);
            if (teacher is null)
            {
                throw new NotFoundException($"User with email:{email} has not found.");
            }

            if (group.TeacherGroups.Any(teacherGroup => teacherGroup.TeacherId == teacher.Id))
            {
                throw new ValidationException("Teacher is already exist with this group.");
            }

            group.TeacherGroups.Add(new TeacherGroup()
            {
                TeacherId = teacher.Id
            });
            _unitOfWork.Groups.UpdateGroup(group);
            await _unitOfWork.SaveChanges();
        }
    }
}
