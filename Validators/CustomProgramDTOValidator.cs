using FluentValidation;

namespace FitnesTracker;

public class CustomProgramDTOCreateValidation : AbstractValidator<CustomProgramCreateDTO>
{
    public CustomProgramDTOCreateValidation()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(1, 200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(1, 200);

        RuleFor(x => x.ExerciseIDs)
            .NotEmpty();
 
        RuleForEach(x => x.ExerciseIDs)
            .GreaterThan(0);

    }
}

public class CustomProgramDTOUpdateValidation : AbstractValidator<CustomProgramUpdateDTO>
{
    public CustomProgramDTOUpdateValidation()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(1, 200)
            .WithMessage("Title must be between 1 to 200 chars");

        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(1, 200)
            .WithMessage("Description must be between 1 to 200 chars");

        RuleFor(x => x.ExerciseIDs)
            .NotEmpty()
            .WithMessage("Id is incorrect");
 
        RuleForEach(x => x.ExerciseIDs)
            .GreaterThan(0)
            .WithMessage("Id is incorrect");
    }
}
