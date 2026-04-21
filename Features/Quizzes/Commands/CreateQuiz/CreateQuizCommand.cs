using ExaminationSystem.Abstractions;
using ExaminationSystem.DTOs.Quizzes;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Commands.CreateQuiz;

public record CreateQuizCommand : IRequest<Result<QuizResponse>>
{
    public Guid DiplomaId { get; init; }
    public string Title { get; init; } = default!;
    public string Instructions { get; init; } = default!;
    public int DurationMinutes { get; init; }
    public double PassScore { get; init; }
    public int? MaxAttempts { get; init; }
} 