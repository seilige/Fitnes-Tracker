using FluentValidation;

namespace FitnesTracker;

public class ExerciseDTOValidator : AbstractValidator<ExerciseRequestDTO>
{
    public ExerciseDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(1, 200)
            .WithMessage("Titel must be between 1 to 200 chars");

        RuleFor(x => x.MuscleGroup)
            .IsInEnum();

        RuleFor(x => x.Sets)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Sets must be greater than 0");

        RuleFor(x => x.Reps)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Reps must be greater than 0");
    }
}
