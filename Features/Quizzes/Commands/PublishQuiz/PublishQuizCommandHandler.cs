using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Interfaces.Repositories;
using ExaminationSystem.DTOs.Quizzes;
using ExaminationSystem.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.Commands.PublishQuiz;

public class PublishQuizCommandHandler(IGenericRepository<Quiz> quizRepository)
    : IRequestHandler<PublishQuizCommand, Result<PublishQuizResponse>>
{
    private readonly IGenericRepository<Quiz> _quizRepository = quizRepository;

    public async Task<Result<PublishQuizResponse>> Handle(PublishQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository
            .GetQueryable()
            .Include(q => q.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);

        if (quiz is null)
            return Result.Failure<PublishQuizResponse>(QuizError.NotFound(request.Id));

        if (quiz.Status == QuizStatus.Published)
            return Result.Failure<PublishQuizResponse>(QuizError.AlreadyPublished(request.Id));

        if (quiz.Questions.Count == 0)
            return Result.Failure<PublishQuizResponse>(QuizError.HasNoQuestions(request.Id));

        foreach (var question in quiz.Questions)
        {
            if (question.Options.Count < 2)
                return Result.Failure<PublishQuizResponse>(QuizError.QuestionHasTooFewOptions(question.Id));

            var correctOptionsCount = question.Options.Count(o => o.IsCorrect);
            if (correctOptionsCount != 1)
                return Result.Failure<PublishQuizResponse>(QuizError.QuestionMustHaveOneCorrectOption(question.Id));
        }

        quiz.Status = QuizStatus.Published;
        quiz.PublishedAt = DateTime.UtcNow;
        quiz.UpdatedAt = DateTime.UtcNow;

        _quizRepository.Update(quiz);
        await _quizRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(new PublishQuizResponse
        {
            QuizId = quiz.Id,
            Status = "published",
            PublishedAt = quiz.PublishedAt
        });
    }
}
