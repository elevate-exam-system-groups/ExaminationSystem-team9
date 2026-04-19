using FluentValidation;

namespace ExaminationSystem.Features.Quizzes.Commands.AddQuestionToQuiz;

public class AddQuestionCommandValidator : AbstractValidator<AddQuestionCommand>
{
    public AddQuestionCommandValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty();

        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.OrderIndex)
            .GreaterThan(0);

        RuleFor(x => x.Explanation)
            .MaximumLength(2000);

        RuleFor(x => x.Options)
            .NotNull()
            .Must(options => options.Count >= 2)
            .WithMessage("Minimum 2 options required.")
            .Must(options => options.Count(option => option.IsCorrect) == 1)
            .WithMessage("Exactly 1 option must have isCorrect = true.");

        RuleForEach(x => x.Options)
            .ChildRules(option =>
            {
                option.RuleFor(x => x.Text)
                    .NotEmpty()
                    .MaximumLength(500);
            });
    }
}
