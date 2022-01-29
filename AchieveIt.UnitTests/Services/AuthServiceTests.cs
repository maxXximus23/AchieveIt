using System;
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
using Microsoft.Extensions.Options;
using NUnit.Framework;
using ValidationException = AchieveIt.BusinessLogic.Exceptions.ValidationException;

namespace AchieveIt.UnitTests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Faker _faker;

        private Fake<IUnitOfWork> _unitOfWork;
        private IMapper _mapper;
        private Fake<IOptions<JwtOptions>> _jwtOptions;
        private Fake<IUsersRepository> _usersRepository;

        [SetUp]
        public void Setup()
        {
            _faker = new Faker();
            _unitOfWork = new Fake<IUnitOfWork>();
            _jwtOptions = new Fake<IOptions<JwtOptions>>();
            _usersRepository = new Fake<IUsersRepository>();

            _mapper = new Mapper(new MapperConfiguration(config =>
            {
                config.AddProfile<UserProfile>();
                config.AddProfile<RoleProfile>();
            }));
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
            AuthUserResultDto result = await sut.RegisterStudent(dto);

            // Assert
            using (new AssertionScope())
            {
                result.Token.Should().NotBeNullOrEmpty();
                result.IsSuccess.Should().BeTrue();
                result.ExpiresOnUtc.ToString("G").Should().Be(DateTime.UtcNow.AddMinutes(fakedOptions.LifeTimeMinutes).ToString("G"));
                result.RefreshToken.Should().BeNull();
                
                _usersRepository.CallsTo(unitOfWork => unitOfWork.IsEmailExist(dto.Email))
                                .MustHaveHappenedOnceExactly();

                _usersRepository.CallsTo(repository => repository.AddUser(A<User>._))
                                .MustHaveHappenedOnceExactly();
                
                _unitOfWork.CallsTo(unitOfWork => unitOfWork.SaveChanges())
                           .MustHaveHappenedOnceExactly();
            }
        }

        private AuthService BuildSut() =>
            new AuthService(
                _unitOfWork.FakedObject,
                _mapper,
                _jwtOptions.FakedObject
            );
    }
}