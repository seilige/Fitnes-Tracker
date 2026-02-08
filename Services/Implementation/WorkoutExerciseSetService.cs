using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Http.Metadata;

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
        return _mapper.Map<WorkoutExerciseSetResponseDTO>(result);
    }

    public async Task<WorkoutExerciseSetResponseDTO> GetSetByIdAsync(int id)
    {
        var entity = await _repository.GetByIDAsync(id);
        return _mapper.Map<WorkoutExerciseSetResponseDTO>(entity);
    }

    public async Task<IEnumerable<WorkoutExerciseSetResponseDTO>> GetSessionSetsAsync(int sessionId)
    {
        var item = await _repository.GetByWorkoutSessionIdAsync(sessionId);
        return _mapper.Map<IEnumerable<WorkoutExerciseSetResponseDTO>>(item);
    }

    public async Task<WorkoutExerciseSetResponseDTO> UpdateSetAsync(int id, WorkoutExerciseSetUpdateDTO dto)
    {
        var entity = await _repository.GetByIDAsync(id);
        
        entity.Weight = dto.Weight;
        entity.Reps = dto.Reps;
        
        await _repository.UpdateAsync(entity);
        return _mapper.Map<WorkoutExerciseSetResponseDTO>(entity); // замени updated на entity
    }

    public async Task DeleteSetAsync(int id)
    {
        await _repository.DeleteByIDAsync(id);
    }
}
