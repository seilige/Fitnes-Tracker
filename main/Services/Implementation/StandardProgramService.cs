using AutoMapper;

namespace FitnesTracker;

public class StandardProgramService : IStandardProgramService
{
    private readonly IStandardProgramRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ExerciseService> _logger;

    public StandardProgramService(IStandardProgramRepository repository, IMapper mapper, ILogger<ExerciseService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Returns a paged list of standard programs by page number and page size.
    /// </summary>
    public async Task<PagedResult<StandardProgramResponseDTO>> GetAllAsync(int pageNumber, int pageSize)
    {
        var pagedUsers = await _repository.GetAllAsync(pageNumber, pageSize);
        var dtos = _mapper.Map<List<StandardProgramResponseDTO>>(pagedUsers.Items);
        return new PagedResult<StandardProgramResponseDTO>(dtos, pagedUsers.TotalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Returns a paged list of standard programs based on pagination params. Throws KeyNotFoundException if page is not found.
    /// </summary>
    public async Task<PagedResult<StandardProgramResponseDTO>> GetPagedAsync(PaginationParams paginationParams)
    {
        var result = await _repository.GetPagedAsync(paginationParams);

        if(result == null) throw new KeyNotFoundException("Page not found");

        var mappedItems = _mapper.Map<List<StandardProgramResponseDTO>>(result.Items);

        return new PagedResult<StandardProgramResponseDTO>(
            mappedItems,
            result.TotalCount,
            result.PageNumber,
            result.PageSize
        );
    }

    /// <summary>
    /// Returns all standard programs without pagination.
    /// </summary>
    public async Task<IEnumerable<StandardProgramResponseDTO>> GetAllAsync()
    {
        var programs = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<StandardProgramResponseDTO>>(programs);
    }

    /// <summary>
    /// Returns a standard program by ID. Throws KeyNotFoundException if the program is not found.
    /// </summary>
    public async Task<StandardProgramResponseDTO?> GetByIdAsync(int id)
    {
        var program = await _repository.GetByIDAsync(id);

        if(program == null) 
        {
            _logger.LogInformation($"Program with id: {id} not found");
            throw new KeyNotFoundException($"Program with id: {id} not found");
        }

        return _mapper.Map<StandardProgramResponseDTO?>(program);
    }
}
