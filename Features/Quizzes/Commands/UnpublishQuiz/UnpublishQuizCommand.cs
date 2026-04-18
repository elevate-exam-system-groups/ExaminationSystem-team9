using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Quizzes;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Commands.UnpublishQuiz;

public record UnpublishQuizCommand(Guid Id) : IRequest<Result<PublishQuizResponse>>;
