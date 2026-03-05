using AutoMapper;
using Moq;
using Xunit;

namespace FitnesTracker.Tests;

public class WorkoutExerciseServiceTests
{
    private readonly Mock<IWorkoutExerciseSetRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<ExerciseService>> _loggerMock;
    private readonly WorkoutExerciseService _service;
    private readonly Mock<IUnitOfWork> _UoW;

    public WorkoutExerciseServiceTests()
    {
        _repositoryMock = new Mock<IWorkoutExerciseSetRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<ExerciseService>>();
        _UoW = new Mock<IUnitOfWork>();

        _service = new WorkoutExerciseService(
            _repositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _UoW.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ReturnsPagedResult()
    {
        var entities = new List<WorkoutExerciseSet> { new WorkoutExerciseSet() };
        var pagedResult = new PagedResult<WorkoutExerciseSet>(entities, 1, 1, 10);
        var dtos = new List<WorkoutExerciseSetResponseDTO> { new WorkoutExerciseSetResponseDTO() };

        _repositoryMock.Setup(r => r.GetAllAsync(1, 10)).ReturnsAsync(pagedResult);
        _mapperMock.Setup(m => m.Map<List<WorkoutExerciseSetResponseDTO>>(entities)).Returns(dtos);

        var result = await _service.GetAllAsync(1, 10);

        Assert.Equal(1, result.TotalCount);
        Assert.Single(result.Items);
    }


    [Fact]
    public async Task AddSetAsync_ReturnsDto()
    {
        var createDto = new WorkoutExerciseSetCreateDTO();
        var entity = new WorkoutExerciseSet();
        var responseDto = new WorkoutExerciseSetResponseDTO();

        _mapperMock.Setup(m => m.Map<WorkoutExerciseSet>(createDto)).Returns(entity);
        _repositoryMock.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<WorkoutExerciseSetResponseDTO>(entity)).Returns(responseDto);

        var result = await _service.AddSetAsync(createDto);

        Assert.Equal(responseDto, result);
        _UoW.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetSetByIdAsync_ReturnsDto_WhenFound()
    {
        var entity = new WorkoutExerciseSet();
        var dto = new WorkoutExerciseSetResponseDTO();

        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<WorkoutExerciseSetResponseDTO>(entity)).Returns(dto);

        var result = await _service.GetSetByIdAsync(1);

        Assert.Equal(dto, result);
    }

    [Fact]
    public async Task GetSetByIdAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        _repositoryMock.Setup(r => r.GetByIDAsync(99)).ReturnsAsync((WorkoutExerciseSet?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetSetByIdAsync(99));
    }


    [Fact]
    public async Task GetSessionSetsAsync_ReturnsDtos_WhenSessionFound()
    {
        var entities = new List<WorkoutExerciseSet> { new WorkoutExerciseSet() };
        var dtos = new List<WorkoutExerciseSetResponseDTO> { new WorkoutExerciseSetResponseDTO() };

        _repositoryMock.Setup(r => r.GetByWorkoutSessionIdAsync(5)).ReturnsAsync(entities);
        _mapperMock.Setup(m => m.Map<IEnumerable<WorkoutExerciseSetResponseDTO>>(entities)).Returns(dtos);

        var result = await _service.GetSessionSetsAsync(5);

        Assert.Single(result);
    }

    [Fact]
    public async Task GetSessionSetsAsync_ThrowsKeyNotFoundException_WhenSessionNotFound()
    {
        _repositoryMock.Setup(r => r.GetByWorkoutSessionIdAsync(99))
            .ReturnsAsync((IEnumerable<WorkoutExerciseSet>?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetSessionSetsAsync(99));
    }


    [Fact]
    public async Task UpdateSetAsync_UpdatesAndReturnsDto_WhenFound()
    {
        var entity = new WorkoutExerciseSet { Weight = 50, Reps = 10 };
        var updateDto = new WorkoutExerciseSetUpdateDTO { Weight = 80, Reps = 12 };
        var responseDto = new WorkoutExerciseSetResponseDTO();

        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<WorkoutExerciseSetResponseDTO>(entity)).Returns(responseDto);

        var result = await _service.UpdateSetAsync(1, updateDto);

        Assert.Equal(80, entity.Weight);
        Assert.Equal(12, entity.Reps);
        _repositoryMock.Verify(r => r.UpdateAsync(entity), Times.Once);
        _UoW.Verify(r => r.SaveChangesAsync(), Times.Once);
        Assert.Equal(responseDto, result);
    }

    [Fact]
    public async Task UpdateSetAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        _repositoryMock.Setup(r => r.GetByIDAsync(99)).ReturnsAsync((WorkoutExerciseSet?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdateSetAsync(99, new WorkoutExerciseSetUpdateDTO()));
    }

    [Fact]
    public async Task DeleteSetAsync_ReturnsTrue_WhenFound()
    {
        var entity = new WorkoutExerciseSet();
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(entity);

        var result = await _service.DeleteSetAsync(1);

        Assert.True(result);
        _repositoryMock.Verify(r => r.DeleteByIDAsync(1), Times.Once);
        _UoW.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteSetAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        _repositoryMock.Setup(r => r.GetByIDAsync(99)).ReturnsAsync((WorkoutExerciseSet?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteSetAsync(99));
    }
}
