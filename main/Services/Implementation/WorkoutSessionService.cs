using AutoMapper;

namespace FitnesTracker;

public class WorkoutSessionService : IWorkoutSessionService
{
    private readonly IWorkoutSessionRepository _repository;
    private readonly IMapper _mapper;
    private readonly IWorkoutExerciseSetRepository _setRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<ExerciseService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public WorkoutSessionService(IWorkoutSessionRepository repository,
                                IWorkoutExerciseSetRepository setRepository,
                                IUserRepository userRepository,
                                IMapper mapper,
                                ILogger<ExerciseService> logger,
                                IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _repository = repository;
        _setRepository = setRepository;
        _mapper = mapper;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<WorkoutSessionResponseDTO>> GetAllAsync(int pageNumber, int pageSize)
    {
        var pagedUsers = await _repository.GetAllAsync(pageNumber, pageSize);
        var dtos = _mapper.Map<List<WorkoutSessionResponseDTO>>(pagedUsers.Items);
        return new PagedResult<WorkoutSessionResponseDTO>(dtos, pagedUsers.TotalCount, pageNumber, pageSize);
    }

    public async Task<PagedResult<WorkoutSessionResponseDTO>> GetUserHistoryAsync(int userId, int pageNumber, int pageSize)
    {
        if(await _userRepository.GetByIDAsync(userId) == null) throw new KeyNotFoundException($"User with id: {userId} not found");
 
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
            throw new InvalidOperationException("Session is already completed");

        session.Status = WorkoutStatus.Completed;

        await _repository.UpdateAsync(session);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<WorkoutSessionResponseDTO>(session);
    }

    public async Task<SetUpdateDTO> UpdateSetAsync(SetUpdateDTO dto)
    {
        // get session
        var set = await _setRepository.GetSetByIdWithSessionAsync(dto.SetId);
 
        if(set == null) throw new KeyNotFoundException($"Set with id: {dto.SetId} not found");

        set.Reps = dto.Reps;
        set.Weight = dto.Weight;

        await _setRepository.UpdateAsync(set);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation($"Set with id: {dto.SetId} already updated");

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
                WorkoutSessionId = entity.WorkoutSessionId,
                ExerciseId = exer.WorkoutExerciseSetId,
                Reps = exer.Reps,
                Weight = exer.Weight
            };

            await _setRepository.AddAsync(set);
        }

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<WorkoutSessionResponseDTO>(entity);
    }

    public async Task<WorkoutSessionResponseDTO> GetSessionByIdAsync(int id)
    {
        var item = await _repository.GetByIDAsync(id);
        if(item == null) throw new KeyNotFoundException($"Session with id: {id} not found");
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
        
        if(session == null) throw new KeyNotFoundException($"Session with id: {id} not found");

        session.Status = status;
        await _repository.UpdateAsync(session);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation($"Session status with id: {id} already updated");

        return _mapper.Map<WorkoutSessionResponseDTO>(session);
    }
}
