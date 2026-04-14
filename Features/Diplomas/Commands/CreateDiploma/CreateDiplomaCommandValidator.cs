using FluentValidation;

namespace ExaminationSystem.Features.Diplomas.Commands.CreateDiploma;

public class CreateDiplomaCommandValidator : AbstractValidator<CreateDiplomaCommand>
{
    public CreateDiplomaCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty().
            Length(3, 200);

        RuleFor(c => c.Description)
            .MaximumLength(500);
    }
}
