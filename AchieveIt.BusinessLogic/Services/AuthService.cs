using System;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.Contracts;
using AchieveIt.BusinessLogic.DTOs.Auth;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.UnitOfWork;
using AchieveIt.Shared.Exceptions;
using AchieveIt.Shared.Options;
using AutoMapper;
using Kirpichyov.FriendlyJwt;
using Kirpichyov.FriendlyJwt.Contracts;
using Kirpichyov.FriendlyJwt.RefreshTokenUtilities;
using Microsoft.Extensions.Options;

namespace AchieveIt.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtOptions _jwtOptions;
        private readonly IJwtTokenVerifier _jwtTokenVerifier;
        private readonly RefreshTokenOptions _refreshTokenOptions;
        private readonly IMapper _mapper;

        public AuthService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IOptions<JwtOptions> jwtOptions,
            IOptions<RefreshTokenOptions> refreshTokenOptions, 
            IJwtTokenVerifier jwtTokenVerifier)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtTokenVerifier = jwtTokenVerifier;
            _refreshTokenOptions = refreshTokenOptions.Value;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<AuthUserResultDto> RegisterStudent(RegisterStudentDto registerStudentDto)
        {
            var student = _mapper.Map<RegisterStudentDto, Student>(registerStudentDto);

            return await RegisterUser(student);
        }

        public async Task<AuthUserResultDto> RegisterTeacher(RegisterTeacherDto registerTeacherDto)
        {
            var teacher = _mapper.Map<RegisterTeacherDto, Teacher>(registerTeacherDto);

            return await RegisterUser(teacher);
        }

        public async Task<AuthUserResultDto> RegisterAdmin(RegisterAdminDto registerAdminDto)
        {
            var admin = _mapper.Map<RegisterAdminDto, Admin>(registerAdminDto);

            return await RegisterUser(admin);
        }

        public async Task<AuthUserResultDto> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
           JwtVerificationResult result = _jwtTokenVerifier.Verify(refreshTokenDto.AccessToken);
           if (!result.IsValid)
           {
               throw new ValidationException("Access token is not valid.");
           }

           RefreshToken refreshToken = await _unitOfWork.RefreshTokens.GetToken(refreshTokenDto.RefreshToken);
           if (refreshToken.ExpireDate < DateTime.UtcNow)
           {
               throw new ValidationException("Refresh token is expired.");
           }

           if (!refreshToken.IsValidate)
           {
               throw new ValidationException("Refresh token is not valid.");
           }

           if (refreshToken.JwtId.ToString() != result.TokenId)
           {
               throw new ValidationException("Access token is not valid.");
           }
           
           User user = await _unitOfWork.Users.GetUser(int.Parse(result.UserId));

           GeneratedTokenInfo generatedTokenInfo = GenerateJwtToken(user);

           var newRefreshToken = GenerateRefreshToken(Guid.Parse(generatedTokenInfo.TokenId));
           
           _unitOfWork.RefreshTokens.AddToken(newRefreshToken);
           _unitOfWork.RefreshTokens.DeleteToken(refreshToken);
           await _unitOfWork.SaveChanges();
           
           return new AuthUserResultDto()
           {
               Token = generatedTokenInfo.Token,
               ExpiresOnUtc = generatedTokenInfo.ExpiresOn,
               IsSuccess = true,
               RefreshToken = newRefreshToken.Id
           };
        }

        public async Task<AuthUserResultDto> SignIn(SignInDto signInDto)
        {
            User user = await _unitOfWork.Users.GetUserByEmail(signInDto.Email);

            if (user is null || !BCrypt.Net.BCrypt.Verify(signInDto.Password, user.Password))
            {
                return new AuthUserResultDto()
                {
                    IsSuccess = false
                };
            }

            var jwtToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken(Guid.Parse(jwtToken.TokenId));
            
            _unitOfWork.RefreshTokens.AddToken(newRefreshToken);
            await _unitOfWork.SaveChanges();
            
            return new AuthUserResultDto()
            {
                Token = jwtToken.Token,
                IsSuccess = true,
                RefreshToken = newRefreshToken.Id,
                ExpiresOnUtc = jwtToken.ExpiresOn
            };
        }

        private GeneratedTokenInfo GenerateJwtToken(User user)
        {
            TimeSpan lifeTime = TimeSpan.FromMinutes(_jwtOptions.LifeTimeMinutes);
           
            JwtTokenBuilder jwtTokenBuilder =
                new JwtTokenBuilder(lifeTime, _jwtOptions.Secret)
                    .WithIssuer(_jwtOptions.Issuer)
                    .WithAudience(_jwtOptions.Audience)
                    .WithUserRolePayloadData(user.Role.ToString())
                    .WithUserIdPayloadData(user.Id.ToString())
                    .WithUserEmailPayloadData(user.Email);
           
            if (user is PersonBase personBase)
            {
                jwtTokenBuilder
                    .WithPayloadData("Name", personBase.Name)
                    .WithPayloadData("Surname", personBase.Surname)
                    .WithPayloadData("Patronymic", personBase.Patronymic);
            }

            return jwtTokenBuilder.Build();
        }

        private RefreshToken GenerateRefreshToken(Guid generatedTokenId)
        {
            return new RefreshToken()
            {
                Id = Guid.NewGuid(),
                JwtId = generatedTokenId,
                ExpireDate = DateTime.UtcNow.AddMonths(_refreshTokenOptions.ExpiresOnMonth),
                IsValidate = true
            };
        }

        private async Task<AuthUserResultDto> RegisterUser(User user)
        {
            if (await _unitOfWork.Users.IsEmailExist(user.Email))
            {
                throw new ValidationException("User with same Email is already exist.");
            }

            _unitOfWork.Users.AddUser(user);
            await _unitOfWork.SaveChanges();

            GeneratedTokenInfo generatedTokenInfo = GenerateJwtToken(user);

            var refreshToken = GenerateRefreshToken(Guid.Parse(generatedTokenInfo.TokenId));

            _unitOfWork.RefreshTokens.AddToken(refreshToken);
            await _unitOfWork.SaveChanges();

            return new AuthUserResultDto()
            {
                Token = generatedTokenInfo.Token,
                ExpiresOnUtc = generatedTokenInfo.ExpiresOn,
                IsSuccess = true,
                RefreshToken = refreshToken.Id
            };
        }
    }
}