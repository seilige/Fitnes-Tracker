using AutoMapper;

namespace FitnesTracker;

public class CustomProgramService : ICustomProgramService
{
    private readonly ICustomProgramRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ExerciseService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CustomProgramService(ICustomProgramRepository repository, IMapper mapper, ILogger<ExerciseService> logger, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new custom program from the provided DTO and assigns the creator.
    /// </summary>
    public async Task<CustomProgramResponseDTO> CreateAsync(CustomProgramCreateV2DTO dto, int creatorId)
    {
        var program = _mapper.Map<CustomProgram>(dto);
        program.CreatorId = creatorId;
        await _repository.AddAsync(program);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Program already added through v2 route");
        return _mapper.Map<CustomProgramResponseDTO>(program);
    }

    /// <summary>
    /// Returns a paged list of all custom programs.
    /// </summary>
    public async Task<PagedResult<CustomProgramResponseDTO>> GetAllAsync(int pageNumber, int pageSize)
    {
        var pagedUsers = await _repository.GetAllAsync(pageNumber, pageSize);
        var dtos = _mapper.Map<List<CustomProgramResponseDTO>>(pagedUsers.Items);
        return new PagedResult<CustomProgramResponseDTO>(dtos, pagedUsers.TotalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Returns all custom programs. May return an empty list.
    /// </summary>
    public async Task<IEnumerable<CustomProgramResponseDTO>> GetAllAsync() // may returns void list
    {
        var programs = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<CustomProgramResponseDTO>>(programs);
    }

    /// <summary>
    /// Creates a new custom program (v1) from the provided DTO and assigns the creator.
    /// </summary>
    public async Task<CustomProgramResponseDTO> CreateAsync(CustomProgramCreateDTO dto, int creatorId)
    {
        var program = _mapper.Map<CustomProgram>(dto);
        program.CreatorId = creatorId;
        await _repository.AddAsync(program);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Program already added");
        return _mapper.Map<CustomProgramResponseDTO>(program);
    }

    /// <summary>
    /// Gets a custom program by ID. Throws KeyNotFoundException if not found.
    /// </summary>
    public async Task<CustomProgramResponseDTO?> GetByIdAsync(int id)
    {
        var program = await _repository.GetByIDAsync(id);

        if(program == null)
        {
            _logger.LogInformation($"Program with id: {id} not found");
            throw new KeyNotFoundException($"Program {id} not found");
        }

        return _mapper.Map<CustomProgramResponseDTO?>(program);
    }

    /// <summary>
    /// Updates a custom program by ID. Throws KeyNotFoundException if not found.
    /// </summary>
    public async Task<CustomProgramResponseDTO> UpdateAsync(int id, CustomProgramUpdateDTO dto)
    {
        var program = _mapper.Map<CustomProgram>(dto);
        program.CustomProgramId = id;
        var updated = await _repository.UpdateAsync(program);

        if(updated == null)
        {
            _logger.LogInformation("Program not found");
            throw new KeyNotFoundException("Updated program not found");
        }

        await _unitOfWork.SaveChangesAsync(); // on every transaction
        return _mapper.Map<CustomProgramResponseDTO>(updated);
    }

    /// <summary>
    /// Deletes a custom program by ID. Throws KeyNotFoundException if not found. Returns true on success.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var program = await _repository.GetByIDAsync(id);

        if(program == null)
        {
            _logger.LogInformation($"Program with id: {id} not found");
            throw new KeyNotFoundException("Not found");
        }

        await _repository.DeleteByIDAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
