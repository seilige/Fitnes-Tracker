using AutoMapper;

namespace FitnesTracker;

public class WorkoutSessionService : IWorkoutSessionService
{
    private readonly IWorkoutSessionRepository _repository;
    private readonly IMapper _mapper;

    public WorkoutSessionService(IWorkoutSessionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<WorkoutSessionResponseDTO> CreateSessionAsync(WorkoutSessionCreateDTO dto)
    {
        var entity = _mapper.Map<WorkoutSession>(dto);
        await _repository.AddAsync(entity);
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
        return _mapper.Map<WorkoutSessionResponseDTO>(session);
    }
}
