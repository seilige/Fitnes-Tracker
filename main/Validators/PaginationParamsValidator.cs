using FluentValidation;

namespace FitnesTracker;

public class PaginationParamsValidator : AbstractValidator<PaginationParams>
{
    public PaginationParamsValidator()
    {
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .ExclusiveBetween(1, 50);

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);
    }
}
