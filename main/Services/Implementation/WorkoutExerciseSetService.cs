using AutoMapper;

namespace FitnesTracker;

public class WorkoutExerciseService : IWorkoutExerciseService
{
    private readonly IWorkoutExerciseSetRepository _repository;
    private readonly IMapper _mapper;

    public WorkoutExerciseService(IWorkoutExerciseSetRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<WorkoutExerciseSetResponseDTO> AddSetAsync(WorkoutExerciseSetCreateDTO dto)
    {
        var entity = _mapper.Map<WorkoutExerciseSet>(dto);
        var result = await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return _mapper.Map<WorkoutExerciseSetResponseDTO>(result);
    }

    public async Task<WorkoutExerciseSetResponseDTO> GetSetByIdAsync(int id)
    {
        var entity = await _repository.GetByIDAsync(id);

        if(entity == null) throw new KeyNotFoundException($"Not found");

        return _mapper.Map<WorkoutExerciseSetResponseDTO>(entity);
    }

    public async Task<IEnumerable<WorkoutExerciseSetResponseDTO>> GetSessionSetsAsync(int sessionId)
    {
        var session = await _repository.GetByWorkoutSessionIdAsync(sessionId);

        if(session == null) throw new KeyNotFoundException($"Session with id: {sessionId} not found");
        
        return _mapper.Map<IEnumerable<WorkoutExerciseSetResponseDTO>>(session);
    }

    public async Task<WorkoutExerciseSetResponseDTO> UpdateSetAsync(int id, WorkoutExerciseSetUpdateDTO dto)
    {
        var entity = await _repository.GetByIDAsync(id);

        if(entity == null) throw new KeyNotFoundException($"Exercise with id: {id} not found");

        entity.Weight = dto.Weight;
        entity.Reps = dto.Reps;

        await _repository.UpdateAsync(entity);
        await _repository.SaveChangesAsync();
        return _mapper.Map<WorkoutExerciseSetResponseDTO>(entity);
    }

    public async Task DeleteSetAsync(int id)
    {
        var exercise = await _repository.GetByIDAsync(id);

        if(exercise == null) throw new KeyNotFoundException($"Exercise with id: {id} not found");

        await _repository.DeleteByIDAsync(id);
        await _repository.SaveChangesAsync();
    }
}
