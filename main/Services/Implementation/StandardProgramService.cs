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

    public async Task<PagedResult<StandardProgramResponseDTO>> GetAllAsync(int pageNumber, int pageSize)
    {
        var pagedUsers = await _repository.GetAllAsync(pageNumber, pageSize);
        var dtos = _mapper.Map<List<StandardProgramResponseDTO>>(pagedUsers.Items);
        return new PagedResult<StandardProgramResponseDTO>(dtos, pagedUsers.TotalCount, pageNumber, pageSize);
    }

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

    public async Task<IEnumerable<StandardProgramResponseDTO>> GetAllAsync()
    {
        var programs = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<StandardProgramResponseDTO>>(programs);
    }

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
