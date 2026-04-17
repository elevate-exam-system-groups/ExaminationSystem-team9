using ExaminationSystem.Abstractions;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Commands.UpdateQuiz
{
    public record UpdateQuizCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Instructions { get; set; } = default!;
        public int DurationMinutes { get; set; }
        public double PassScore { get; set; }
        public int? MaxAttempts { get; set; }

    }
}
