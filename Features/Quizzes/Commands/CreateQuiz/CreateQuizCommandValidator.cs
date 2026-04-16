using FluentValidation;

namespace ExaminationSystem.Features.Quizzes.Commands.CreateQuiz
{
    public class CreateQuizCommandValidator : AbstractValidator<CreateQuizCommand>

    {
        public CreateQuizCommandValidator()
        {
            RuleFor(x => x.DiplomaId).NotEmpty();

            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);

            RuleFor(x => x.Instructions).MaximumLength(1000);

            RuleFor(x=> x.DurationMinutes).GreaterThan(0);

            RuleFor(x => x.PassScore).InclusiveBetween(0, 100);

            RuleFor(x => x.MaxAttempts).GreaterThan(0).When(x => x.MaxAttempts.HasValue);

        }
    }
}
