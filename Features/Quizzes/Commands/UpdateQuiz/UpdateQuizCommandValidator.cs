using FluentValidation;

namespace ExaminationSystem.Features.Quizzes.Commands.UpdateQuiz
{
    public class UpdateQuizCommandValidator : AbstractValidator<UpdateQuizCommand>
    {
        public UpdateQuizCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Title).NotEmpty().Length(3,200);
            RuleFor(x => x.Instructions).MaximumLength(1000);
            RuleFor(x => x.DurationMinutes).GreaterThan(0);
            RuleFor(x => x.PassScore).InclusiveBetween(0, 100);
            RuleFor(x => x.MaxAttempts).GreaterThan(0).When(x => x.MaxAttempts.HasValue);
        }
    }
}
