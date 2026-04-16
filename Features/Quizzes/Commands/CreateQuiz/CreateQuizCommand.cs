
using ExaminationSystem.Features.Quizzes.Common;

namespace ExaminationSystem.Features.Quizzes.Commands.CreateQuiz;

public record CreateQuizCommand(
    Guid DiplomaId,
    string Title,
    string? Instructions,
    int DurationMinutes,
    double PassScore,
    int? MaxAttempts
) : MediatR.IRequest<ExaminationSystem.Abstractions.Result<QuizResponse>>;