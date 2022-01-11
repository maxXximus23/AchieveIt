using System;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Auth;
using AchieveIt.BusinessLogic.Exceptions;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.UnitOfWork;
using Kirpichyov.FriendlyJwt;

namespace AchieveIt.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthUserDto> RegisterStudent(RegisterStudentDto registerStudentDto)
        {
            if (await _unitOfWork.Users.IsEmailExist(registerStudentDto.Email))
                throw new ValidationException("User with same Email is already exist!");

            var user = new User()
            {
                Birthday = registerStudentDto.Birthday,
                Email = registerStudentDto.Email,
                Group = registerStudentDto.Group,
                Name = registerStudentDto.Name,
                Password = BCrypt.Net.BCrypt.HashPassword(registerStudentDto.Password),
                Surname = registerStudentDto.Surname,
                Patronymic = registerStudentDto.Patronymic
            };
            
            _unitOfWork.Users.AddUser(user);
            await _unitOfWork.SaveChanges();
            
            TimeSpan lifeTime = TimeSpan.FromMinutes(1);
            string secret = "SecretYGPV8XC6bPJhQCUBV2LtDSharp";
            
            GeneratedTokenInfo generatedTokenInfo =
                new JwtTokenBuilder(lifeTime, secret)
                    .WithIssuer("https://localhost:5001")
                    .WithAudience("https://localhost:5001")
                    .WithPayloadData("Name", user.Name)
                    .WithPayloadData("Surname", user.Surname)
                    .WithPayloadData("Patronymic", user.Patronymic)
                    .WithUserIdPayloadData(user.Id.ToString())
                    .WithUserEmailPayloadData(user.Email)
                    .Build();

            return new AuthUserDto()
            {
                Token = generatedTokenInfo.Token,
                IsSuccess = true,
                RefreshToken = null
            };
        }

        public Task<AuthUserDto> RegisterTeacher()
        {
            throw new System.NotImplementedException();
        }

        public Task<AuthUserDto> RegisterAdmin()
        {
            throw new System.NotImplementedException();
        }
    }
}