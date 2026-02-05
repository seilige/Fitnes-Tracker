using AutoMapper;

namespace FitnesTracker;

public class StandardProgramService : IStandardProgramService
{
    private IStandardProgramRepository _repository;
    private IMapper _mapper;

    public StandardProgramService(IStandardProgramRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StandardProgramResponseDTO>> GetAllAsync()
    {
        var programs = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<StandardProgramResponseDTO>>(programs);
    }

    public async Task<StandardProgramResponseDTO?> GetByIdAsync(int id)
    {
        var program = await _repository.GetByIDAsync(id);
        return _mapper.Map<StandardProgramResponseDTO?>(program);
    }
}
