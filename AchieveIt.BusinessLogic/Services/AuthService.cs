using System;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Auth;
using AchieveIt.BusinessLogic.Exceptions;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.UnitOfWork;
using AutoMapper;
using Kirpichyov.FriendlyJwt;

namespace AchieveIt.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AuthUserDto> RegisterStudent(RegisterStudentDto registerStudentDto)
        {
            var student = _mapper.Map<RegisterStudentDto, Student>(registerStudentDto);

            return await RegisterUser(student);
        }

        public async Task<AuthUserDto> RegisterTeacher(RegisterTeacherDto registerTeacherDto)
        {
            var teacher = _mapper.Map<RegisterTeacherDto, Teacher>(registerTeacherDto);

            return await RegisterUser(teacher);
        }

        public async Task<AuthUserDto> RegisterAdmin(RegisterAdminDto registerAdminDto)
        {
            var admin = _mapper.Map<RegisterAdminDto, Admin>(registerAdminDto);

            return await RegisterUser(admin);
        }

        private async Task<AuthUserDto> RegisterUser(User user)
        {
            if (await _unitOfWork.Users.IsEmailExist(user.Email))
                throw new ValidationException("User with same Email is already exist!");
            
            _unitOfWork.Users.AddUser(user);
            await _unitOfWork.SaveChanges();
            
            TimeSpan lifeTime = TimeSpan.FromMinutes(1);
            string secret = "SecretYGPV8XC6bPJhQCUBV2LtDSharp";
            
            JwtTokenBuilder jwtTokenBuilder =
                new JwtTokenBuilder(lifeTime, secret)
                    .WithIssuer("https://localhost:5001")
                    .WithAudience("https://localhost:5001")
                    .WithUserRolePayloadData(user.Role.ToString())
                    .WithUserIdPayloadData(user.Id.ToString())
                    .WithUserEmailPayloadData(user.Email);

            if (user is Student student)
            {
                jwtTokenBuilder
                    .WithPayloadData("Name", student.Name)
                    .WithPayloadData("Surname", student.Surname)
                    .WithPayloadData("Patronymic", student.Patronymic);
            }
            
            if (user is Teacher teacher)
            {
                jwtTokenBuilder
                    .WithPayloadData("Name", teacher.Name)
                    .WithPayloadData("Surname", teacher.Surname)
                    .WithPayloadData("Patronymic", teacher.Patronymic);
            }

            GeneratedTokenInfo generatedTokenInfo = jwtTokenBuilder.Build();

            return new AuthUserDto()
            {
                Token = generatedTokenInfo.Token,
                IsSuccess = true,
                RefreshToken = null
            };
        }
    }
}