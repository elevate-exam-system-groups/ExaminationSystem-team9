using ExaminationSystem.Abstractions;   
using ExaminationSystem.Domain.DTOs.QuizResponse;

namespace ExaminationSystem.Features.Quizzes.Commands.CreateQuiz;

public record CreateQuizCommand(
    Guid DiplomaId,
    string Title,
    string? Instructions,
    int DurationMinutes,
    double PassScore,
    int? MaxAttempts
) : MediatR.IRequest<Result<QuizResponse>>;