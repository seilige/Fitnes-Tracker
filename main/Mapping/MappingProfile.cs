using AutoMapper;

namespace FitnesTracker;

public class Mapper : Profile
{
    public Mapper()
    {
        // Client -> DTO -> Server -> Object -> DB
        CreateMap<UserCreateDTO, User>();
        // DB -> Object(User) -> Server -> DTO(UserResponce) -> Client
        CreateMap<User, UserResponseDTO>();
        CreateMap<UserUpdateDTO, User>();
        CreateMap<Exercise, ExerciseResponseDTO>();
        CreateMap<ExerciseRequestDTO, Exercise>();
        CreateMap<StandardProgram, StandardProgramResponseDTO>();
        CreateMap<CustomProgramCreateDTO, CustomProgram>();
        CreateMap<CustomProgramCreateV2DTO, CustomProgram>();

        // Automapper can map Creator to CreatorId and Exercise to int, but it's incorrect
        CreateMap<CustomProgram, CustomProgramResponseDTO>()
            .ForMember(dest => dest.ExerciseIDs, opt => opt.MapFrom(src => src.Exercises.Select(x => x.ExId)))
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Creator != null ? src.Creator.IdUser : (int?)null));

        CreateMap<CustomProgramUpdateDTO, CustomProgram>();
        CreateMap<WorkoutExerciseSetCreateDTO, WorkoutExerciseSet>();
        CreateMap<WorkoutExerciseSet, WorkoutExerciseSetResponseDTO>();
        CreateMap<WorkoutExerciseSetUpdateDTO, WorkoutExerciseSet>();
        CreateMap<WorkoutSessionCreateDTO, WorkoutSession>();
        CreateMap<WorkoutSession, WorkoutSessionResponseDTO>();
        CreateMap<WorkoutSessionUpdateDTO, WorkoutSession>();

        CreateMap<SetUpdateDTO, WorkoutExerciseSet>();
        CreateMap<WorkoutExerciseSet, SetUpdateDTO>();
    
        CreateMap<WorkoutSession, WorkoutSessionResponseDTO>()
            .ForMember(dest => dest.SessionId, opt => opt.MapFrom(x => x.SessionId));

        CreateMap<WorkoutExerciseSet, ExerciseSetDTO>()
            .ForMember(dest => dest.SetId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ExerciseName, opt => opt.MapFrom(src => src.Exercise.Title));
    }
}
