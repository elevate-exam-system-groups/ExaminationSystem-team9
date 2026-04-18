using FluentValidation;

namespace ExaminationSystem.Features.Quizzes.Commands.UnpublishQuiz;

public class UnpublishQuizCommandValidator : AbstractValidator<UnpublishQuizCommand>
{
    public UnpublishQuizCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
