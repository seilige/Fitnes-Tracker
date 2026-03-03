using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;

namespace FitnesTracker;

public class StandardProgramServiceTests
{
    private readonly Mock<IStandardProgramRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<ExerciseService>> _mockLogger;
    private readonly StandardProgramService _service;

    public StandardProgramServiceTests()
    {
        _mockRepo = new Mock<IStandardProgramRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<ExerciseService>>();
        _service = new StandardProgramService(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllPagedAsync_ReturnsPagedResult()
    {
        var programs = new List<StandardProgram> { new StandardProgram { ProgramId = 1, Title = "Program 1" } };
        var dtos = new List<StandardProgramResponseDTO> { new StandardProgramResponseDTO { StandardProgramId = 1, Title = "Program 1" } };
        var pagedResult = new PagedResult<StandardProgram>(programs, 1, 1, 10);

        _mockRepo.Setup(r => r.GetAllAsync(1, 10)).ReturnsAsync(pagedResult);
        _mockMapper.Setup(m => m.Map<List<StandardProgramResponseDTO>>(programs)).Returns(dtos);

        var result = await _service.GetAllAsync(1, 10);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsPagedResult()
    {
        var paginationParams = new ExerciseQueryParameters { PageNumber = 1, PageSize = 5 };
        var programs = new List<StandardProgram> { new StandardProgram { ProgramId = 1, Title = "Program 1" } };
        var dtos = new List<StandardProgramResponseDTO> { new StandardProgramResponseDTO { StandardProgramId = 1, Title = "Program 1" } };
        var pagedResult = new PagedResult<StandardProgram>(programs, 1, 1, 5);

        _mockRepo.Setup(r => r.GetPagedAsync(paginationParams)).ReturnsAsync(pagedResult);
        _mockMapper.Setup(m => m.Map<List<StandardProgramResponseDTO>>(programs)).Returns(dtos);

        var result = await _service.GetPagedAsync(paginationParams);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.TotalCount.Should().Be(1);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(5);
    }

    [Fact]
    public async Task GetPagedAsync_ThrowsKeyNotFoundException_WhenResultIsNull()
    {
        var paginationParams = new ExerciseQueryParameters { PageNumber = 99, PageSize = 5 };

        _mockRepo.Setup(r => r.GetPagedAsync(paginationParams)).ReturnsAsync((PagedResult<StandardProgram>?)null);

        var act = async () => await _service.GetPagedAsync(paginationParams);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Page not found");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllPrograms()
    {
        var programs = new List<StandardProgram>
        {
            new StandardProgram { ProgramId = 1, Title = "Program 1" },
            new StandardProgram { ProgramId = 2, Title = "Program 2" }
        };
        var dtos = new List<StandardProgramResponseDTO>
        {
            new StandardProgramResponseDTO { StandardProgramId = 1, Title = "Program 1" },
            new StandardProgramResponseDTO { StandardProgramId = 2, Title = "Program 2" }
        };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(programs);
        _mockMapper.Setup(m => m.Map<IEnumerable<StandardProgramResponseDTO>>(programs)).Returns(dtos);

        var result = await _service.GetAllAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsDto_WhenProgramExists()
    {
        var program = new StandardProgram { ProgramId = 1, Title = "Program 1" };
        var dto = new StandardProgramResponseDTO { StandardProgramId = 1, Title = "Program 1" };

        _mockRepo.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(program);
        _mockMapper.Setup(m => m.Map<StandardProgramResponseDTO?>(program)).Returns(dto);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.StandardProgramId.Should().Be(1);
        result.Title.Should().Be("Program 1");
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsKeyNotFoundException_WhenProgramNotFound()
    {
        _mockRepo.Setup(r => r.GetByIDAsync(999)).ReturnsAsync((StandardProgram?)null);

        var act = async () => await _service.GetByIdAsync(999);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Program with id: 999 not found");
    }
}
