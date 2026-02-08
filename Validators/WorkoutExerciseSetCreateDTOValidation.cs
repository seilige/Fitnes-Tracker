using FluentValidation;

namespace FitnesTracker;

public class WorkoutExerciseSetCreateDTOValidation : AbstractValidator<WorkoutExerciseSetCreateDTO>
{
    public WorkoutExerciseSetCreateDTOValidation()
    {
        RuleFor(x => x.Reps)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Reps must be greater then 0");

        RuleFor(x => x.Weight)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Weight must be greater then 0");

        RuleFor(x => x.WorkoutSessionId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Incorrect data");
    }
}
