using AutoMapper;

namespace FitnesTracker;

public class ExerciseService : IExerciseService
{
    private IExerciseRepository _repository;
    private IMapper _mapper;

    public ExerciseService(IExerciseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<ExerciseResponseDTO>> GetPagedAsync(PaginationParams paginationParams)
    {
        var result = await _repository.GetPagedAsync(paginationParams);
        
        var mappedItems = _mapper.Map<List<ExerciseResponseDTO>>(result.Items);
        
        return new PagedResult<ExerciseResponseDTO>(
            mappedItems, 
            result.TotalCount,
            result.PageNumber,
            result.PageSize
        );
    }

    public async Task<PagedResult<ExerciseResponseDTO>> GetFilteredExercisesAsync(ExerciseQueryParameters parameters)
    {
        var result = await _repository.GetFilteredExercisesAsync(parameters);

        var mappedItems = _mapper.Map<List<ExerciseResponseDTO>>(result.Items);

        return new PagedResult<ExerciseResponseDTO>(
            mappedItems, 
            result.TotalCount,
            result.PageNumber,
            result.PageSize
        );
    }

    public async Task<ExerciseResponseDTO?> GetByTitleAsync(string title)
    {
        var exercise = await _repository.GetByTitleAsync(title);
        return _mapper.Map<ExerciseResponseDTO?>(exercise);
    }

    public async Task<IEnumerable<ExerciseResponseDTO>> GetByMuscleGroupAsync(MuscleGroup group)
    {
        var exercises = await _repository.GetByMuscleGroupAsync(group);
        return _mapper.Map<IEnumerable<ExerciseResponseDTO>>(exercises);
    }

    public async Task<IEnumerable<ExerciseResponseDTO>> GetAllAsync()
    {
        var Exercise = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ExerciseResponseDTO>>(Exercise);
    }

    public async Task<ExerciseResponseDTO> CreateAsync(ExerciseRequestDTO dto)
    {
        var item = _mapper.Map<Exercise>(dto);
        await _repository.AddAsync(item);
        await _repository.SaveChangesAsync();
        return _mapper.Map<ExerciseResponseDTO>(item);
    }

    public async Task<ExerciseResponseDTO?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIDAsync(id);
        return _mapper.Map<ExerciseResponseDTO?>(entity);
    }

    public async Task<ExerciseResponseDTO> UpdateAsync(int id, ExerciseRequestDTO dto)
    {
        var item = _mapper.Map<Exercise>(dto);
        item.ExId = id;
        var entity = await _repository.UpdateAsync(item);
        await _repository.SaveChangesAsync();
        return _mapper.Map<ExerciseResponseDTO>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _repository.DeleteByIDAsync(id);
        await _repository.SaveChangesAsync();
        return true;
    }
}
