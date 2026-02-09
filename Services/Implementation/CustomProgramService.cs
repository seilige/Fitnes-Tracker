using AutoMapper;

namespace FitnesTracker;

public class CustomProgramService : ICustomProgramService
{
    private readonly ICustomProgramRepository _repository;
    private readonly IMapper _mapper;

    public CustomProgramService(ICustomProgramRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomProgramResponseDTO>> GetAllAsync()
    {
        var programs = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<CustomProgramResponseDTO>>(programs);
    }

    public async Task<CustomProgramResponseDTO> CreateAsync(CustomProgramCreateDTO dto, int creatorId)
    {
        var program = _mapper.Map<CustomProgram>(dto);
        program.CreatorId = creatorId;
        await _repository.AddAsync(program);
        await _repository.SaveChangesAsync();
        return _mapper.Map<CustomProgramResponseDTO>(program);
    }

    public async Task<CustomProgramResponseDTO?> GetByIdAsync(int id)
    {
        var program = await _repository.GetByIDAsync(id);
        return _mapper.Map<CustomProgramResponseDTO?>(program);
    }

    public async Task<CustomProgramResponseDTO> UpdateAsync(int id, CustomProgramUpdateDTO dto)
    {
        var program = _mapper.Map<CustomProgram>(dto);
        program.CustProgId = id;
        var updated = await _repository.UpdateAsync(program);
        await _repository.SaveChangesAsync(); // on every transaction
        return _mapper.Map<CustomProgramResponseDTO>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _repository.DeleteByIDAsync(id);
        await _repository.SaveChangesAsync();
        return true;
    }
}
