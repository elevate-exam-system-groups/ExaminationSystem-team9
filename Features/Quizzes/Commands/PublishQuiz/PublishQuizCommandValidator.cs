using FluentValidation;

namespace ExaminationSystem.Features.Quizzes.Commands.PublishQuiz;

public class PublishQuizCommandValidator : AbstractValidator<PublishQuizCommand>
{
    public PublishQuizCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
