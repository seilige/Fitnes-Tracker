using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace FitnesTracker;

public class UserCreateDTOValidator : AbstractValidator<UserCreateDTO>
{
    public UserCreateDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 10)
            .Matches(@"^[a-zA-Zа-яА-ЯёЁ]+$");

        RuleFor(x => x.Lastname)
            .NotEmpty()
            .Length(1, 10)
            .Matches(@"^[a-zA-Zа-яА-ЯёЁ]+$");
    }
}

public class UserUpdateDTOValidator : AbstractValidator<UserUpdateDTO>
{
    public UserUpdateDTOValidator()
    {
        RuleFor(x => x.IdUser)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Id is incorrect");

        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 10)
            .Matches(@"^[a-zA-Zа-яА-ЯёЁ]+$")
            .WithMessage("Name must be between 1 to 200 chars");

        RuleFor(x => x.Lastname)
            .NotEmpty()
            .Length(1, 10)
            .Matches(@"^[a-zA-Zа-яА-ЯёЁ]+$")
            .WithMessage("Lastname must be between 1 to 200 chars");
    }
}
