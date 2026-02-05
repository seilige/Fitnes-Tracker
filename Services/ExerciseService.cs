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
        return _mapper.Map<ExerciseResponseDTO>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _repository.DeleteByIDAsync(id);
        return true;
    }
}
