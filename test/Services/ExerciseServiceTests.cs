using Xunit;
using FluentValidation.TestHelper;

namespace FitnesTracker;

public class ExerciseUpdateDTOValidatorTests
{
    private readonly ExerciseDTOValidator _validator;

    public ExerciseUpdateDTOValidatorTests()
    {
        _validator = new ExerciseDTOValidator();
    }

    [Fact]
    public async Task ExerciseValidatorSuccessTest()
    {
        ExerciseRequestDTO ex = new ExerciseRequestDTO{Title = "validTitle", MuscleGroup = MuscleGroup.Back, Sets = 3, Reps = 10};

        var result = _validator.TestValidate(ex);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task ExerciseValidatorErrorTest()
    {
        ExerciseRequestDTO ex = new ExerciseRequestDTO{MuscleGroup = MuscleGroup.Back, Sets = 3, Reps = 10};

        var result = _validator.TestValidate(ex);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }
}
