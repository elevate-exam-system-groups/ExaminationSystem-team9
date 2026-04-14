using FluentValidation;

namespace ExaminationSystem.Features.Diplomas.Commands.UpdateDiploma;

public class UpdateDiplomaCommandValidator : AbstractValidator<UpdateDiplomaCommand>
{
    public UpdateDiplomaCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty().
            Length(3, 100);

        RuleFor(c => c.Description)
            .MaximumLength(500);
    }
}
