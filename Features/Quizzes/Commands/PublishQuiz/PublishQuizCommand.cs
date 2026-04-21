using ExaminationSystem.Abstractions;
using ExaminationSystem.DTOs.Quizzes;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Commands.PublishQuiz;

public record PublishQuizCommand(Guid Id) : IRequest<Result<PublishQuizResponse>>;
