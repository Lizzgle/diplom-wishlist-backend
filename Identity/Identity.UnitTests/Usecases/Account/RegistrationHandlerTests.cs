using AutoMapper;
using Core.Exceptions;
using FluentAssertions;
using Identity.Application.Usecases.Account.Commands.Registration;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using Moq;
using ArgumentException = Core.Exceptions.ArgumentException;

namespace Identity.UnitTests.Usecases.Account;

public class RegistrationHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly RegistrationHandler _handler;

    public RegistrationHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        _mapperMock = new Mock<IMapper>();
        _handler = new RegistrationHandler(_userManagerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowAlreadyExistException_WhenEmailAlreadyRegistered()
    {
        // Arrange
        var request = new RegistrationRequest
        {
            Email = "test@example.com",
            UserName = "testuser",
            DateOfBirth = DateTime.Now.AddYears(-20),
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        var userList = new List<User>
        {
            new User { Email = "test@example.com", EmailConfirmed = true }
        }.AsQueryable().BuildMock();
        
        _userManagerMock.Setup(m => m.Users).Returns(userList);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AlreadyExistException>().WithMessage("Already exist entity. test@example.com is already registered");
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenPasswordsDoNotMatch()
    {
        // Arrange
        var request = new RegistrationRequest
        {
            Email = "test2@example.com",
            UserName = "testuser2",
            DateOfBirth = DateTime.Now.AddYears(-20),
            Password = "Password123!",
            ConfirmPassword = "DifferentPassword123!"
        };

        var userList = new List<User>
        {
            new User { Email = "test@example.com", EmailConfirmed = true }
        }.AsQueryable().BuildMock();
        
        _userManagerMock.Setup(m => m.Users).Returns(userList);
        
        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Argument {0} is invalid. Passwords do not match");
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new RegistrationRequest
        {
            Email = "newuser@example.com",
            UserName = "newuser",
            DateOfBirth = DateTime.Now.AddYears(-20),
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        var userList = new List<User> { }.AsQueryable().BuildMock();
        
        _userManagerMock.Setup(m => m.Users).Returns(userList);
        _mapperMock.Setup(m => m.Map<User>(request)).Returns(new User { Email = request.Email, UserName = request.UserName });
        _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), request.Password)).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<User>(), "user")).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<User>())).ReturnsAsync("test-code");

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.Email.Should().Be(request.Email);
        response.Code.Should().Be("test-code");
    }
}