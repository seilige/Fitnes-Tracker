using System.Xml;
using AutoMapper;

namespace FitnesTracker;

public class WorkoutSessionService : IWorkoutSessionService
{
    private readonly IWorkoutSessionRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICustomProgramRepository _customProgramRepository;
    private readonly IStandardProgramRepository _standardProgramRepository;
    private readonly IWorkoutExerciseSetRepository _setRepository;

    public WorkoutSessionService(IWorkoutSessionRepository repository,
                                ICustomProgramRepository customProgramRepository,
                                IStandardProgramRepository standardProgramRepository,
                                IWorkoutExerciseSetRepository setRepository,
                                IMapper mapper)
    {
        _repository = repository;
        _customProgramRepository = customProgramRepository;
        _standardProgramRepository = standardProgramRepository;
        _setRepository = setRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<WorkoutSessionResponseDTO>> GetUserHistoryAsync(int userId, int pageNumber, int pageSize)
    {
        var pagedSessions = await _repository.GetUserSessionsAsync(userId, pageNumber, pageSize);
        
        var dtos = _mapper.Map<List<WorkoutSessionResponseDTO>>(pagedSessions.Items);
        
        return new PagedResult<WorkoutSessionResponseDTO>(
            dtos, pagedSessions.TotalCount, pageNumber, pageSize
        );
    }

    public async Task<WorkoutSessionResponseDTO> CompleteSessionAsync(int sessionId)
    {
        var session = await _repository.GetByIdWithDetailsAsync(sessionId);

        if (session.Status == WorkoutStatus.Completed)
            throw new InvalidOperationException("Сессия уже завершена");

        session.Status = WorkoutStatus.Completed;

        await _repository.UpdateAsync(session);
        await _repository.SaveChangesAsync();

        return _mapper.Map<WorkoutSessionResponseDTO>(session);
    }

    public async Task<SetUpdateDTO> UpdateSetAsync(SetUpdateDTO dto)
    {
        // get session
        var set = await _setRepository.GetSetByIdWithSessionAsync(dto.Id);
 
        set.Reps = dto.Reps;
        set.Weight = dto.Weight;

        await _setRepository.UpdateAsync(set);
        await _repository.SaveChangesAsync();

        return _mapper.Map<SetUpdateDTO>(set);
    }

    public async Task<WorkoutSessionResponseDTO> CreateSessionAsync(WorkoutSessionCreateDTO dto)
    {
        var entity = _mapper.Map<WorkoutSession>(dto); // session
        entity.Status = WorkoutStatus.InProgress;
        await _repository.AddAsync(entity);

        // session need exersice sets:
        foreach(var exer in dto.WorkoutExerciseSets)
        {
            var set = new WorkoutExerciseSet
            {
                WorkoutSessionId = entity.SessionId,
                ExerciseId = exer.ExerciseId,
                Reps = exer.Reps,
                Weight = exer.Weight
            };

            await _setRepository.AddAsync(set);
        }

        await _repository.SaveChangesAsync();

        return _mapper.Map<WorkoutSessionResponseDTO>(entity);
    }

    public async Task<WorkoutSessionResponseDTO> GetSessionByIdAsync(int id)
    {
        var item = await _repository.GetByIDAsync(id);
        return _mapper.Map<WorkoutSessionResponseDTO>(item);
    }

    public async Task<IEnumerable<WorkoutSessionResponseDTO>> GetUserSessionsAsync(int userId)
    {
        var sessions = await _repository.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<WorkoutSessionResponseDTO>>(sessions);
    }

    public async Task<WorkoutSessionResponseDTO> UpdateSessionStatusAsync(int id, WorkoutStatus status)
    {
        var session = await _repository.GetByIDAsync(id);
        session.Status = status;
        await _repository.UpdateAsync(session);
        await _repository.SaveChangesAsync();
        return _mapper.Map<WorkoutSessionResponseDTO>(session);
    }
}
