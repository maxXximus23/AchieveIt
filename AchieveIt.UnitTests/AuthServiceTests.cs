using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AchieveIt.BusinessLogic.DTOs.Auth;
using AchieveIt.BusinessLogic.Profiles;
using AchieveIt.BusinessLogic.Services;
using AchieveIt.DataAccess.Entities;
using AchieveIt.DataAccess.Repositories.Contracts;
using AchieveIt.DataAccess.UnitOfWork;
using AchieveIt.Shared.Options;
using AutoMapper;
using Bogus;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using Kirpichyov.FriendlyJwt;
using Kirpichyov.FriendlyJwt.Constants;
using Kirpichyov.FriendlyJwt.Contracts;
using Kirpichyov.FriendlyJwt.RefreshTokenUtilities;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using UserProfile = AchieveIt.BusinessLogic.Profiles.UserProfile;
using ValidationException = AchieveIt.Shared.Exceptions.ValidationException;

namespace AuthServiceTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AuthServiceTests
    {
        private Faker _faker;

        private Fake<IUnitOfWork> _unitOfWork;
        private IMapper _mapper;
        private Fake<IOptions<JwtOptions>> _jwtOptions;
        private JwtOptions fakedOptions;
        private Fake<IUsersRepository> _usersRepository;
        private Fake<IRefreshTokenRepository> _refreshTokenRepository;
        private Fake<IOptions<RefreshTokenOptions>> _refreshTokenOptions;
        private Fake<IJwtTokenVerifier> _jwtTokenVerifier;

        [SetUp]
        public void Setup()
        {
            _faker = new Faker();
            _unitOfWork = new Fake<IUnitOfWork>();
            _jwtOptions = new Fake<IOptions<JwtOptions>>();
            _usersRepository = new Fake<IUsersRepository>();
            _refreshTokenRepository = new Fake<IRefreshTokenRepository>();
            _refreshTokenOptions = new Fake<IOptions<RefreshTokenOptions>>();
            _jwtTokenVerifier = new Fake<IJwtTokenVerifier>();

            _mapper = new Mapper(new MapperConfiguration(config =>
            {
                config.AddProfile<UserProfile>();
                config.AddProfile<RoleProfile>();
            }));
            
            fakedOptions = new JwtOptions
            {
                Secret = _faker.Random.AlphaNumeric(32),
                LifeTimeMinutes = 30,
                Issuer = _faker.Internet.Url(),
                Audience = _faker.Internet.Url()
            };
            
            _jwtOptions.CallsTo(options => options.Value)
                .Returns(fakedOptions);
        }

        [Test]
        public async Task RegisterStudent_EmailIsAlreadyRegistered_ShouldThrowValidationException()
        {
            // Arrange
            var dto = new RegisterStudentDto
            {
                Email = _faker.Internet.Email()
            };

            _unitOfWork.CallsTo(unitOfWork => unitOfWork.Users)
                        .Returns(_usersRepository.FakedObject);

            _usersRepository.CallsTo(unitOfWork => unitOfWork.IsEmailExist(dto.Email))
                            .Returns(true);

            AuthService sut = BuildSut();

            // Act
            Func<Task<AuthUserResultDto>> func = () => sut.RegisterStudent(dto);

            // Assert
            using (new AssertionScope())
            {
                await func.Should().ThrowExactlyAsync<ValidationException>();

                _usersRepository.CallsTo(unitOfWork => unitOfWork.IsEmailExist(dto.Email))
                                .MustHaveHappenedOnceExactly();
            }
        }
        
        [Test]
        public async Task RegisterStudent_EmailIsNotRegistered_ShouldBeRegisteredSuccessfully()
        {
            // Arrange
            var dto = new RegisterStudentDto
            {
                Email = _faker.Internet.Email(),
                Name = _faker.Person.FirstName,
                Surname = _faker.Person.LastName,
                Patronymic = _faker.Person.LastName,
                Role = _faker.Random.Enum<RoleDto>(),
                Birthday = _faker.Person.DateOfBirth,
                Password = _faker.Internet.Password()
            };

            _unitOfWork.CallsTo(unitOfWork => unitOfWork.Users)
                       .Returns(_usersRepository.FakedObject);

            _usersRepository.CallsTo(unitOfWork => unitOfWork.IsEmailExist(dto.Email))
                            .Returns(false);

            AuthService sut = BuildSut();

            // Act
            AuthUserResultDto result = await sut.RegisterStudent(dto);

            // Assert
            using (new AssertionScope())
            {
                result.Token.Should().NotBeNullOrEmpty();
                result.RefreshToken.Should().NotBeEmpty();
                result.IsSuccess.Should().BeTrue();
                result.ExpiresOnUtc!.Value
                    .ToString("yyyy-MM-dd hh:mm")
                    .Should()
                    .Be(DateTime.UtcNow.AddMinutes(fakedOptions.LifeTimeMinutes)
                        .ToString("yyyy-MM-dd hh:mm"));

                _usersRepository.CallsTo(unitOfWork => unitOfWork.IsEmailExist(dto.Email))
                                .MustHaveHappenedOnceExactly();

                _usersRepository.CallsTo(repository => repository.AddUser(A<User>._))
                                .MustHaveHappenedOnceExactly();

                _unitOfWork.CallsTo(unitOfWork => unitOfWork.SaveChanges())
                    .MustHaveHappenedTwiceExactly();
            }
        }

        [Test]
        public async Task RegisterTeacher_EmailIsAlreadyRegistered_ShouldThrowValidationException()
        {
            // Arrange
            var dto = new RegisterTeacherDto()
            {
                Email = _faker.Internet.Email()
            };

            _unitOfWork.CallsTo(unitOfWork => unitOfWork.Users)
                .Returns(_usersRepository.FakedObject);

            _usersRepository.CallsTo(unitOfWork => unitOfWork.IsEmailExist(dto.Email))
                .Returns(true);

            AuthService sut = BuildSut();

            // Act
            Func<Task<AuthUserResultDto>> func = () => sut.RegisterTeacher(dto);

            // Assert
            using (new AssertionScope())
            {
                await func.Should().ThrowExactlyAsync<ValidationException>();

                _usersRepository.CallsTo(unitOfWork => unitOfWork.IsEmailExist(dto.Email))
                    .MustHaveHappenedOnceExactly();
            }
        }

        [Test]
        public async Task RegisterTeacher_EmailIsNotRegistered_ShouldBeRegisteredSuccessfully()
        {
            // Arrange
            var dto = new RegisterTeacherDto
            {
                Email = _faker.Internet.Email(),
                Name = _faker.Person.FirstName,
                Surname = _faker.Person.LastName,
                Patronymic = _faker.Person.LastName,
                Role = _faker.Random.Enum<RoleDto>(),
                Birthday = _faker.Person.DateOfBirth,
                Password = _faker.Internet.Password()
            };

            var fakedOptions = new JwtOptions
            {
                Secret = _faker.Random.AlphaNumeric(32),
                LifeTimeMinutes = 30,
                Issuer = _faker.Internet.Url(),
                Audience = _faker.Internet.Url()
            };

            _unitOfWork.CallsTo(unitOfWork => unitOfWork.Users)
                       .Returns(_usersRepository.FakedObject);

            _usersRepository.CallsTo(unitOfWork => unitOfWork.IsEmailExist(dto.Email))
                            .Returns(false);

            _jwtOptions.CallsTo(options => options.Value)
                       .Returns(fakedOptions);

            AuthService sut = BuildSut();

            // Act
            AuthUserResultDto result = await sut.RegisterTeacher(dto);

            // Assert
            using (new AssertionScope())
            {
                result.Token.Should().NotBeNullOrEmpty();
                result.RefreshToken.Should().NotBeEmpty();
                result.IsSuccess.Should().BeTrue();
                result.ExpiresOnUtc!.Value
                    .ToString("yyyy-MM-dd hh:mm")
                    .Should()
                    .Be(DateTime.UtcNow.AddMinutes(fakedOptions.LifeTimeMinutes)
                        .ToString("yyyy-MM-dd hh:mm"));

                _usersRepository.CallsTo(unitOfWork => unitOfWork.IsEmailExist(dto.Email))
                                .MustHaveHappenedOnceExactly();

                _usersRepository.CallsTo(repository => repository.AddUser(A<User>._))
                                .MustHaveHappenedOnceExactly();

                _unitOfWork.CallsTo(unitOfWork => unitOfWork.SaveChanges())
                    .MustHaveHappenedTwiceExactly();
            }
        }
        
        [Test]
        public async Task RegisterAdmin_EmailIsAlreadyRegistered_ShouldThrowValidationException()
        {
            // Arrange
            var dto = new RegisterAdminDto()
            {
                Email = _faker.Internet.Email()
            };

            _unitOfWork.CallsTo(unitOfWork => unitOfWork.Users)
                .Returns(_usersRepository.FakedObject);

            _usersRepository.CallsTo(unitOfWork => unitOfWork.IsEmailExist(dto.Email))
                .Returns(true);

            AuthService sut = BuildSut();

            // Act
            Func<Task<AuthUserResultDto>> func = () => sut.RegisterAdmin(dto);

            // Assert
            using (new AssertionScope())
            {
                await func.Should().ThrowExactlyAsync<ValidationException>();

                _usersRepository.CallsTo(unitOfWork => unitOfWork.IsEmailExist(dto.Email))
                    .MustHaveHappenedOnceExactly();
            }
        }
        
        [Test]
        public async Task RegisterAdmin_EmailIsNotRegistered_ShouldBeRegisteredSuccessfully()
        {
            // Arrange
            var dto = new RegisterAdminDto
            {
                Email = _faker.Internet.Email(),
                Role = _faker.Random.Enum<RoleDto>(),
                Password = _faker.Internet.Password()
            };

            var fakedOptions = new JwtOptions
            {
                Secret = _faker.Random.AlphaNumeric(32),
                LifeTimeMinutes = 30,
                Issuer = _faker.Internet.Url(),
                Audience = _faker.Internet.Url()
            };

            _unitOfWork.CallsTo(unitOfWork => unitOfWork.Users)
                       .Returns(_usersRepository.FakedObject);

            _usersRepository.CallsTo(unitOfWork => unitOfWork.IsEmailExist(dto.Email))
                            .Returns(false);

            _jwtOptions.CallsTo(options => options.Value)
                       .Returns(fakedOptions);

            AuthService sut = BuildSut();

            // Act
            AuthUserResultDto result = await sut.RegisterAdmin(dto);

            // Assert
            using (new AssertionScope())
            {
                result.Token.Should().NotBeNullOrEmpty();
                result.RefreshToken.Should().NotBeEmpty();
                result.IsSuccess.Should().BeTrue();
                result.ExpiresOnUtc!.Value
                    .ToString("yyyy-MM-dd hh:mm")
                    .Should()
                    .Be(DateTime.UtcNow.AddMinutes(fakedOptions.LifeTimeMinutes)
                        .ToString("yyyy-MM-dd hh:mm"));

                _usersRepository.CallsTo(unitOfWork => unitOfWork.IsEmailExist(dto.Email))
                                .MustHaveHappenedOnceExactly();

                _usersRepository.CallsTo(repository => repository.AddUser(A<User>._))
                                .MustHaveHappenedOnceExactly();

                _unitOfWork.CallsTo(unitOfWork => unitOfWork.SaveChanges())
                    .MustHaveHappenedTwiceExactly();
            }
        }

        [Test]
        public async Task SignIn_EmailIsNotRegistered_ShouldNotBeSignIn()
        {
            // Arrange
            SignInDto signInDto = new SignInDto()
            {
                Email = _faker.Internet.Email(),
                Password = _faker.Internet.Password()
            };
            User userInRepository = null;
            
            _unitOfWork.CallsTo(unitOfWork => unitOfWork.Users)
                .Returns(_usersRepository.FakedObject);
            
            _usersRepository.CallsTo(repository => repository.GetUserByEmail(signInDto.Email))
                .Returns(userInRepository);

            AuthService sut = BuildSut();

            // Act
            AuthUserResultDto result = await sut.SignIn(signInDto);
            
            // Assert
            using (new AssertionScope())
            {
                result.Token.Should().BeNull();
                result.RefreshToken.Should().BeNull();
                result.IsSuccess.Should().BeFalse();
                result.ExpiresOnUtc.Should().BeNull();

                _usersRepository.CallsTo(repository => repository.GetUserByEmail(signInDto.Email))
                    .MustHaveHappenedOnceExactly();
                
                _unitOfWork.CallsTo(unitOfWork => unitOfWork.SaveChanges())
                    .MustNotHaveHappened();
            }
        }

        [Test]
        public async Task SignIn_PasswordIsInvalid_ShouldNotBeSignIn()
        {
            // Arrange
            SignInDto signInDto = new SignInDto()
            {
                Email = _faker.Internet.Email(),
                Password = _faker.Internet.Password()
            };
            
            User userInRepository = new Student()
            {
                Email = signInDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(signInDto.Password + _faker.Random.AlphaNumeric(3))
            };
            
            _unitOfWork.CallsTo(unitOfWork => unitOfWork.Users)
                .Returns(_usersRepository.FakedObject);
            
            
            _usersRepository.CallsTo(repository => repository.GetUserByEmail(signInDto.Email))
                .Returns(userInRepository);
            
            AuthService sut = BuildSut();

            // Act
            AuthUserResultDto result = await sut.SignIn(signInDto);
            
            // Assert
            using (new AssertionScope())
            {
                result.Token.Should().BeNull();
                result.RefreshToken.Should().BeNull();
                result.IsSuccess.Should().BeFalse();
                result.ExpiresOnUtc.Should().BeNull();

                _usersRepository.CallsTo(repository => repository.GetUserByEmail(signInDto.Email))
                    .MustHaveHappenedOnceExactly();
                
                _unitOfWork.CallsTo(unitOfWork => unitOfWork.SaveChanges())
                    .MustNotHaveHappened();
            }
            
        }

        [Test]
        public async Task SignIn_PasswordIsValid_ShouldSignIn()
        {
            // Arrange
            SignInDto signInDto = new SignInDto()
            {
                Email = _faker.Internet.Email(),
                Password = _faker.Internet.Password()
            };
            
            Student userInRepository = CreateStudent(signInDto.Email, signInDto.Password);
            
            _unitOfWork.CallsTo(unitOfWork => unitOfWork.Users)
                .Returns(_usersRepository.FakedObject);
            
            
            _usersRepository.CallsTo(repository => repository.GetUserByEmail(signInDto.Email))
                .Returns(userInRepository);
            
            _unitOfWork.CallsTo(unitOfWork => unitOfWork.RefreshTokens)
                .Returns(_refreshTokenRepository.FakedObject);
            
            AuthService sut = BuildSut();

            // Act
            AuthUserResultDto result = await sut.SignIn(signInDto);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var token = jwtSecurityTokenHandler.ReadJwtToken(result.Token);

            // Assert
            using (new AssertionScope())
            {
                result.Token.Should().NotBeNull();
                result.RefreshToken.Should().NotBeNull();
                result.IsSuccess.Should().BeTrue();
                result.ExpiresOnUtc!.Value
                    .ToString("yyyy-MM-dd hh:mm")
                    .Should()
                    .Be(DateTime.UtcNow.AddMinutes(fakedOptions.LifeTimeMinutes)
                        .ToString("yyyy-MM-dd hh:mm"));

                _usersRepository.CallsTo(repository => repository.GetUserByEmail(signInDto.Email))
                    .MustHaveHappenedOnceExactly();
                
                _refreshTokenRepository.CallsTo(tokenRepository => tokenRepository.AddToken(A<RefreshToken>._))
                    .MustHaveHappenedOnceExactly();

                _unitOfWork.CallsTo(unitOfWork => unitOfWork.SaveChanges())
                    .MustHaveHappenedOnceExactly();

                token.Claims.Should().Contain(claim => claim.Type == PayloadDataKeys.TokenId &&
                                                       claim.Value == token.Id);
                
                token.Claims.Should().Contain(claim => claim.Type == PayloadDataKeys.UserId &&
                                                       claim.Value == userInRepository.Id.ToString());
            
                token.Claims.Should().Contain(claim => claim.Type == PayloadDataKeys.UserEmail &&
                                                       claim.Value == userInRepository.Email);
            
                token.Claims.Should().Contain(claim => claim.Type == "role" &&
                                                       claim.Value == userInRepository.Role.ToString());
            
                token.Claims.Should().Contain(claim => claim.Type == "user_name" &&
                                                       claim.Value == userInRepository.Name);
            
                token.Claims.Should().Contain(claim => claim.Type == "user_surname" &&
                                                       claim.Value == userInRepository.Surname);
            
                token.Claims.Should().Contain(claim => claim.Type == "user_patronymic" &&
                                                       claim.Value == userInRepository.Patronymic);
            }
        }

        [Test]
        public async Task RefreshToken_AccessTokenIsInvalid_ShouldThrowValidationException()
        {
            // Arrange
            RefreshTokenDto refreshTokenDto = new RefreshTokenDto();
            JwtVerificationResult jwtVerificationResult = new JwtVerificationResult()
            {
                IsValid = false
            };

            _jwtTokenVerifier.CallsTo(tokenVerifier => tokenVerifier.Verify(refreshTokenDto.AccessToken, null, null))
                .Returns(jwtVerificationResult);
            
            AuthService sut = BuildSut();

            // Act
            Func<Task<AuthUserResultDto>> func = () => sut.RefreshToken(refreshTokenDto);

            // Assert
            using (new AssertionScope())
            {
                await func.Should().ThrowExactlyAsync<ValidationException>();
            }
        }
        
        [Test]
        public async Task RefreshToken_RefreshTokenExpireDateIsLessThanUtcNow_ShouldThrowValidationException()
        {
            // Arrange
            RefreshTokenDto refreshTokenDto = new RefreshTokenDto();
            RefreshToken refreshToken = new RefreshToken()
            {
                ExpireDate = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(3))
            };
            
            JwtVerificationResult jwtVerificationResult = new JwtVerificationResult()
            {
                IsValid = true
            };
            
            _unitOfWork.CallsTo(unitOfWork => unitOfWork.RefreshTokens)
                .Returns(_refreshTokenRepository.FakedObject);

            _jwtTokenVerifier.CallsTo(tokenVerifier => tokenVerifier.Verify(refreshTokenDto.AccessToken, null, null))
                .Returns(jwtVerificationResult);
            
            _refreshTokenRepository.CallsTo(tokenRepository => tokenRepository.GetToken(refreshTokenDto.RefreshToken))
                .Returns(refreshToken);
            
            AuthService sut = BuildSut();

            // Act
            Func<Task<AuthUserResultDto>> func = () => sut.RefreshToken(refreshTokenDto);

            // Assert
            using (new AssertionScope())
            {
                await func.Should().ThrowExactlyAsync<ValidationException>();
            }
        }
        
        [Test]
        public async Task RefreshToken_RefreshTokenIsInvalid_ShouldThrowValidationException()
        {
            // Arrange
            RefreshTokenDto refreshTokenDto = new RefreshTokenDto();
            RefreshToken refreshToken = new RefreshToken()
            {
                IsValidate = false,
                ExpireDate = DateTime.UtcNow.Add(TimeSpan.FromMinutes(3))
            };
            
            JwtVerificationResult jwtVerificationResult = new JwtVerificationResult()
            {
                IsValid = true
            };
            
            _unitOfWork.CallsTo(unitOfWork => unitOfWork.RefreshTokens)
                .Returns(_refreshTokenRepository.FakedObject);

            _jwtTokenVerifier.CallsTo(tokenVerifier => tokenVerifier.Verify(refreshTokenDto.AccessToken, null, null))
                .Returns(jwtVerificationResult);
            
            _refreshTokenRepository.CallsTo(tokenRepository => tokenRepository.GetToken(refreshTokenDto.RefreshToken))
                .Returns(refreshToken);
            
            AuthService sut = BuildSut();

            // Act
            Func<Task<AuthUserResultDto>> func = () => sut.RefreshToken(refreshTokenDto);

            // Assert
            using (new AssertionScope())
            {
                await func.Should().ThrowExactlyAsync<ValidationException>();
            }
        }
        
        [Test]
        public async Task RefreshToken_JwtIdInRefreshTokenIsInvalid_ShouldThrowValidationException()
        {
            // Arrange
            RefreshTokenDto refreshTokenDto = new RefreshTokenDto();
            RefreshToken refreshToken = new RefreshToken()
            {
                IsValidate = true,
                JwtId = _faker.Random.Guid(),
                ExpireDate = DateTime.UtcNow.Add(TimeSpan.FromMinutes(3))
            };
            
            JwtVerificationResult jwtVerificationResult = new JwtVerificationResult()
            {
                TokenId = _faker.Random.Guid().ToString(),
                IsValid = true
            };
            
            _unitOfWork.CallsTo(unitOfWork => unitOfWork.RefreshTokens)
                .Returns(_refreshTokenRepository.FakedObject);

            _jwtTokenVerifier.CallsTo(tokenVerifier => tokenVerifier.Verify(refreshTokenDto.AccessToken, null, null))
                .Returns(jwtVerificationResult);
            
            _refreshTokenRepository.CallsTo(tokenRepository => tokenRepository.GetToken(refreshTokenDto.RefreshToken))
                .Returns(refreshToken);
            
            AuthService sut = BuildSut();

            // Act
            Func<Task<AuthUserResultDto>> func = () => sut.RefreshToken(refreshTokenDto);

            // Assert
            using (new AssertionScope())
            {
                await func.Should().ThrowExactlyAsync<ValidationException>();
            }
        }
        
        [Test]
        public async Task RefreshToken_RefreshTokenIsValid_RequiredServicesShouldBeCalled()
        {
            // Arrange
            User user = CreateStudent();
            RefreshTokenDto refreshTokenDto = new RefreshTokenDto();
            RefreshToken refreshToken = new RefreshToken()
            {
                IsValidate = true,
                JwtId = _faker.Random.Guid(),
                ExpireDate = DateTime.UtcNow.Add(TimeSpan.FromMinutes(3))
            };
            
            JwtVerificationResult jwtVerificationResult = new JwtVerificationResult()
            {
                UserId = _faker.Random.Int(0).ToString(),
                TokenId = refreshToken.JwtId.ToString(),
                IsValid = true
            };
            
            _unitOfWork.CallsTo(unitOfWork => unitOfWork.RefreshTokens)
                .Returns(_refreshTokenRepository.FakedObject);
            
            _unitOfWork.CallsTo(unitOfWork => unitOfWork.Users)
                .Returns(_usersRepository.FakedObject);

            _usersRepository.CallsTo(usersRepository => usersRepository.GetUser(int.Parse(
                    jwtVerificationResult.UserId)))
                .Returns(user);

            _jwtTokenVerifier.CallsTo(tokenVerifier => tokenVerifier.Verify(refreshTokenDto.AccessToken, null, null))
                .Returns(jwtVerificationResult);
            
            _refreshTokenRepository.CallsTo(tokenRepository => tokenRepository.GetToken(refreshTokenDto.RefreshToken))
                .Returns(refreshToken);
            
            AuthService sut = BuildSut();

            // Act
            AuthUserResultDto result = await sut.RefreshToken(refreshTokenDto);;

            // Assert
            using (new AssertionScope())
            {
                _refreshTokenRepository.CallsTo(refreshToken => refreshToken.AddToken(A<RefreshToken>._))
                    .MustHaveHappenedOnceExactly();
                
                _refreshTokenRepository.CallsTo(refreshToken => refreshToken.DeleteToken(A<RefreshToken>._))
                    .MustHaveHappenedOnceExactly();

                _unitOfWork.CallsTo(unitOfWork => unitOfWork.SaveChanges())
                    .MustHaveHappenedOnceExactly();
            }
        }

        private Student CreateStudent(string email = null, string password = null)
        {
            return new Student()
            {
                Id = _faker.Random.Int(0),
                Patronymic = _faker.Person.LastName,
                Surname = _faker.Person.LastName,
                Name = _faker.Person.FirstName,
                Role = _faker.Random.Enum<Role>(),
                Email = email ?? _faker.Internet.Email(),
                Password = BCrypt.Net.BCrypt.HashPassword(password ?? _faker.Internet.Password())
            };
        }
        
        private AuthService BuildSut() =>
            new AuthService(
                _unitOfWork.FakedObject,
                _mapper,
                _jwtOptions.FakedObject,
                _refreshTokenOptions.FakedObject,
                _jwtTokenVerifier.FakedObject
            );
    }
}