using AutoMapper;

namespace FitnesTracker;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<UserCreateDTO, User>();
        CreateMap<User, UserResponseDTO>();
        CreateMap<UserUpdateDTO, User>();
        CreateMap<Exercise, ExerciseResponseDTO>();
        CreateMap<ExerciseRequestDTO, Exercise>();
        CreateMap<StandardProgram, StandardProgramResponseDTO>();
        CreateMap<CustomProgramCreateDTO, CustomProgram>();

        // Automapper can map Creator to CreatorId and Exercise to int, but it's incorrect
CreateMap<CustomProgram, CustomProgramResponseDTO>()
    .ForMember(dest => dest.ExerciseIDs, opt => opt.MapFrom(src => src.Exercises.Select(x => x.ExId)))
    .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Creator != null ? src.Creator.IdUser : (int?)null));

        CreateMap<CustomProgramUpdateDTO, CustomProgram>();
    }
}
