using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Quizzes;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Interfaces.Repositories;
using ExaminationSystem.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.Commands.UnpublishQuiz;

public class UnpublishQuizCommandHandler(
    IGenericRepository<Quiz> quizRepository,
    IGenericRepository<QuizAttempt> quizAttemptRepository)
    : IRequestHandler<UnpublishQuizCommand, Result<PublishQuizResponse>>
{
    private readonly IGenericRepository<Quiz> _quizRepository = quizRepository;
    private readonly IGenericRepository<QuizAttempt> _quizAttemptRepository = quizAttemptRepository;

    public async Task<Result<PublishQuizResponse>> Handle(UnpublishQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.Id, cancellationToken);

        if (quiz is null)
            return Result.Failure<PublishQuizResponse>(QuizError.NotFound(request.Id));

        if (quiz.Status == DiplomaStatus.Draft)
            return Result.Failure<PublishQuizResponse>(QuizError.AlreadyDraft(request.Id));

        var hasInProgressAttempts = await _quizAttemptRepository
            .GetQueryable()
            .AnyAsync(a => a.QuizId == request.Id && a.Status == QuizAttemptStatus.InProgress, cancellationToken);

        if (hasInProgressAttempts)
            return Result.Failure<PublishQuizResponse>(QuizError.HasInProgressAttempts(request.Id));

        quiz.Status = DiplomaStatus.Draft;
        quiz.PublishedAt = null;
        quiz.UpdatedAt = DateTime.UtcNow;

        _quizRepository.Update(quiz);
        await _quizRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(new PublishQuizResponse
        {
            QuizId = quiz.Id,
            Status = "draft",
            PublishedAt = quiz.PublishedAt
        });
    }
}
