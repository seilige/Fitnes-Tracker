using AutoMapper;

namespace FitnesTracker;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ExerciseService> _logger;

    public ExerciseService(IExerciseRepository repository, IMapper mapper, ILogger<ExerciseService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Returns a paged list of all exercises.
    /// </summary>
    public async Task<PagedResult<ExerciseResponseDTO>> GetAllAsync(int pageNumber, int pageSize)
    {
        var pagedUsers = await _repository.GetAllAsync(pageNumber, pageSize);
        var dtos = _mapper.Map<List<ExerciseResponseDTO>>(pagedUsers.Items);
        return new PagedResult<ExerciseResponseDTO>(dtos, pagedUsers.TotalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Returns a paged list of exercises using pagination params. Throws KeyNotFoundException if nothing is found.
    /// </summary>
    public async Task<PagedResult<ExerciseResponseDTO>> GetPagedAsync(PaginationParams paginationParams)
    {
        var result = await _repository.GetPagedAsync(paginationParams);

        if(result == null)
        {
            _logger.LogInformation("Program not found");
            throw new KeyNotFoundException($"Program not found");
        }

        var mappedItems = _mapper.Map<List<ExerciseResponseDTO>>(result.Items);
        
        return new PagedResult<ExerciseResponseDTO>(
            mappedItems,
            result.TotalCount,
            result.PageNumber,
            result.PageSize
        );
    }

    /// <summary>
    /// Returns a filtered paged list of exercises based on query parameters. Throws KeyNotFoundException if nothing is found.
    /// </summary>
    public async Task<PagedResult<ExerciseResponseDTO>> GetFilteredExercisesAsync(ExerciseQueryParameters parameters)
    {
        var result = await _repository.GetFilteredExercisesAsync(parameters);

        if(result == null)
        {
            _logger.LogInformation("Exercise not found");
            throw new KeyNotFoundException($"Exerice not found");
        }

        var mappedItems = _mapper.Map<List<ExerciseResponseDTO>>(result.Items);

        return new PagedResult<ExerciseResponseDTO>(
            mappedItems, 
            result.TotalCount,
            result.PageNumber,
            result.PageSize
        );
    }

    /// <summary>
    /// Gets an exercise by title. Throws KeyNotFoundException if not found.
    /// </summary>
    public async Task<ExerciseResponseDTO?> GetByTitleAsync(string title)
    {
        var exercise = await _repository.GetByTitleAsync(title);

        if(exercise == null)
        {
            _logger.LogInformation("Exercise not found");
            throw new KeyNotFoundException($"Exercise not found");
        }

        return _mapper.Map<ExerciseResponseDTO?>(exercise);
    }


    /// <summary>
    /// Gets all exercises by muscle group. Throws KeyNotFoundException if none found.
    /// </summary>
    public async Task<IEnumerable<ExerciseResponseDTO>> GetByMuscleGroupAsync(MuscleGroup group)
    {
        var exercises = await _repository.GetByMuscleGroupAsync(group);

        if(exercises == null)
        {
            _logger.LogInformation("Exercise not found");
            throw new KeyNotFoundException($"Exercise not found");
        }
        
        return _mapper.Map<IEnumerable<ExerciseResponseDTO>>(exercises);
    }

    /// <summary>
    /// Returns all exercises. May return an empty list.
    /// </summary>
    public async Task<IEnumerable<ExerciseResponseDTO>> GetAllAsync() // may returns void list
    {
        var Exercise = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ExerciseResponseDTO>>(Exercise);
    }

    /// <summary>
    /// Creates a new exercise from the provided DTO.
    /// </summary>
    public async Task<ExerciseResponseDTO> CreateAsync(ExerciseRequestDTO dto)
    {
        var item = _mapper.Map<Exercise>(dto);
        await _repository.AddAsync(item);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Exercise already added");
        return _mapper.Map<ExerciseResponseDTO>(item);
    }

    /// <summary>
    /// Gets an exercise by ID. Throws KeyNotFoundException if not found.
    /// </summary>
    public async Task<ExerciseResponseDTO?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIDAsync(id);

        if(entity == null)
        {
            _logger.LogInformation("Exercise not found");
            throw new KeyNotFoundException($"Exercise not found");
        }

        return _mapper.Map<ExerciseResponseDTO?>(entity);
    }

    /// <summary>
    /// Updates an exercise by ID. Throws KeyNotFoundException if not found.
    /// </summary>
    public async Task<ExerciseResponseDTO> UpdateAsync(int id, ExerciseRequestDTO dto)
    {
        var item = _mapper.Map<Exercise>(dto);
        item.ExerciseId = id;
        var entity = await _repository.UpdateAsync(item);
        await _repository.SaveChangesAsync();
        return _mapper.Map<ExerciseResponseDTO>(entity);
    }

    /// <summary>
    /// Deletes an exercise by ID. Throws KeyNotFoundException if not found. Returns true on success.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repository.GetByIDAsync(id);

        if(entity == null)
        {
            _logger.LogInformation("Not found");
            throw new KeyNotFoundException("Not found");
        }

        await _repository.DeleteByIDAsync(id);
        await _repository.SaveChangesAsync();
        return true;
    }
}
