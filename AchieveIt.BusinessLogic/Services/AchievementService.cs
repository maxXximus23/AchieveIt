using System.Collections.Generic;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Achievement;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.UnitOfWork;
using AchieveIt.Shared.Exceptions;
using AutoMapper;

namespace AchieveIt.BusinessLogic.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public AchievementService(IMapper mapper, IUnitOfWork unitOfWork, IFileService fileService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<AchievementDto> CreateAchievement(CreateAchievementDto createAchievementDto)
        {
            if (await _unitOfWork.Achievements.IsAchievementExist(createAchievementDto.Name))
            {
                throw new ValidationException($"Achievement with name: {createAchievementDto.Name} is already exist.");
            }
            
            var achievement = _mapper.Map<CreateAchievementDto, Achievement>(createAchievementDto);
            achievement.Url = await _fileService.UploadIcon(createAchievementDto.Icon);
            
            await _unitOfWork.Achievements.CreateAchievement(achievement);
            await _unitOfWork.SaveChanges();

            return _mapper.Map<Achievement, AchievementDto>(achievement);
        }

        public async Task<AchievementUserDto> CreateStudentAchievement(
            CreateStudentAchievementDto createStudentAchievementDto, int studentId)
        {
            var achievementUser =
                _mapper.Map<CreateStudentAchievementDto, AchievementUser>(createStudentAchievementDto);
            if (await _unitOfWork.Achievements.IsStudentHaveAchievement(
                studentId, createStudentAchievementDto.AchievementId))
            {
                throw new ValidationException($"Student with id: {studentId} is already have same achievement.");
            }

            achievementUser.UserId = studentId;

            await _unitOfWork.Users.GetUser<Student>(achievementUser.UserId);
            await _unitOfWork.Achievements.GetAchievementById(achievementUser.AchievementId);

            await _unitOfWork.Achievements.CreateStudentAchievement(achievementUser);
            await _unitOfWork.SaveChanges();
            return _mapper.Map<AchievementUser, AchievementUserDto>(achievementUser);
        }

        public async Task<AchievementDto> UpdateAchievement(UpdateAchievementDto updateAchievementDto, 
            int achievementId)
        {
            var achievement = await _unitOfWork.Achievements.GetAchievementById(achievementId);

            _mapper.Map(updateAchievementDto, achievement);
            achievement.Url = await _fileService.UploadIcon(updateAchievementDto.Icon);
            
            _unitOfWork.Achievements.UpdateAchievement(achievement);
            await _unitOfWork.SaveChanges();

            return _mapper.Map<Achievement, AchievementDto>(achievement);
        }

        public async Task DeleteAchievementById(int achievementId)
        {
            await _unitOfWork.Achievements.DeleteAchievementById(achievementId);
            await _unitOfWork.SaveChanges();
        }

        public async Task DeleteStudentAchievementById(DeleteStudentAchievementDto deleteStudentAchievementDto, int studentId)
        {
            await _unitOfWork.Achievements.DeleteStudentAchievement(deleteStudentAchievementDto.AchievementId,
                studentId);
            await _unitOfWork.SaveChanges();
        }

        public async Task<IReadOnlyCollection<AchievementDto>> GetAchievements()
        {
            var achievements = await _unitOfWork.Achievements.GetAchievements();
            return _mapper.Map<IReadOnlyCollection<Achievement>, IReadOnlyCollection<AchievementDto>>(achievements);
        }

        public async Task<AchievementDto> GetAchievementById(int achievementId)
        {
            var achievement = await _unitOfWork.Achievements.GetAchievementById(achievementId);
            return _mapper.Map<Achievement, AchievementDto>(achievement);
        }

        public async Task<IReadOnlyCollection<AchievementDto>> GetStudentAchievements(int studentId)
        {
            var achievements = await _unitOfWork.Achievements.GetStudentAchievements(studentId);
            return _mapper.Map<IReadOnlyCollection<Achievement>, IReadOnlyCollection<AchievementDto>>(achievements);
        }
    }
}