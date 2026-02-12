using AutoMapper;

namespace FitnesTracker;

public class StandardProgramService : IStandardProgramService
{
    private readonly IStandardProgramRepository _repository;
    private readonly IMapper _mapper;

    public StandardProgramService(IStandardProgramRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
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

        if(program == null) throw new KeyNotFoundException($"Program with id: {id} not found");

        return _mapper.Map<StandardProgramResponseDTO?>(program);
    }
}
