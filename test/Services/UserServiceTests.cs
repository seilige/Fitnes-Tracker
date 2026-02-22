using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;

namespace FitnesTracker;

public class UserServiceTests
{
    private Mock<IUserRepository> _mockRepo;
    private Mock<IMapper> _mockMapper;
    private readonly UserService _service;
    private readonly Mock<IEmailService> _emailService;
    private readonly Mock<ILogger<ExerciseService>> _mockLogger;

    public UserServiceTests()
    {
        _mockLogger = new Mock<ILogger<ExerciseService>>();
        _mockRepo = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _emailService = new Mock<IEmailService>();
        _service = new UserService(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _emailService.Object);
    }
 
    [Fact]
    public async Task GetUserByIdTest()
    {
        var user = new User { UserId = 1, Name = "Test User" };
        var userDto = new UserResponseDTO { UserId = 1, Name = "Test User" };
        
        _mockRepo.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(user);
        _mockMapper.Setup(m => m.Map<UserResponseDTO?>(user)).Returns(userDto);

        var result = await _service.GetByIdAsync(1);
        
        result.Should().NotBeNull();
        result.UserId.Should().Be(1);
    }

    [Fact]
    public async Task GetUserByIdFalseTest() // user is exists with no setup him
    {
        _mockRepo.Setup(r => r.GetByIDAsync(999)).ReturnsAsync((User?)null);
    
        var act = async () => await _service.GetByIdAsync(999);
        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("User with id: 999 not found");
    }

    [Fact]
    public async Task UserUpdateSuccessTest()
    {
        var userDto = new UserUpdateDTO { Name = "New name" };
        var user = new User { UserId = 1, Name = "Test User" };
        var resultDto = new UserResponseDTO { UserId = 1, Name = "Updated" };

        _mockMapper.Setup(m => m.Map<User>(userDto)).Returns(new User { Name = "Updated" });
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(user);
        _mockMapper.Setup(m => m.Map<UserResponseDTO>(user)).Returns(resultDto);
    }

    [Fact]
    public async Task UserUpdateErrorTest()
    {
        var dto = new UserUpdateDTO { Name = "Updated" };
        
        _mockMapper.Setup(m => m.Map<User>(dto)).Returns(new User());
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<User>()))
            .ThrowsAsync(new Exception("DB error"));
        
        var act = async () => await _service.UpdateAsync(1, dto);
        
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("DB error");
    }
}
