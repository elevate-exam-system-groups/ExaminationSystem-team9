using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Interfaces.Repositories;
using ExaminationSystem.DTOs.Questions;
using ExaminationSystem.Errors;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Commands.AddQuestionToQuiz;

public class AddQuestionCommandHandler(
    IGenericRepository<Quiz> quizRepository,
    IGenericRepository<Question> questionRepository)
    : IRequestHandler<AddQuestionCommand, Result<AddQuestionResponse>>
{
    private readonly IGenericRepository<Quiz> _quizRepository = quizRepository;
    private readonly IGenericRepository<Question> _questionRepository = questionRepository;

    public async Task<Result<AddQuestionResponse>> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.QuizId, cancellationToken);

        if (quiz is null)
            return Result.Failure<AddQuestionResponse>(QuizError.NotFound(request.QuizId));

        var question = new Question
        {
            Id = Guid.NewGuid(),
            QuizId = request.QuizId,
            Text = request.Text,
            Explanation = request.Explanation,
            OrderIndex = request.OrderIndex,
            CreatedAt = DateTime.UtcNow,
            Options = request.Options.Select(option => new Option
            {
                Id = Guid.NewGuid(),
                Text = option.Text,
                IsCorrect = option.IsCorrect,
                CreatedAt = DateTime.UtcNow
            }).ToList()
        };

        await _questionRepository.AddAsync(question, cancellationToken);
        var affectedRows = await _questionRepository.SaveChangesAsync(cancellationToken);

        if (affectedRows <= 0)
            return Result.Failure<AddQuestionResponse>(new Error("Question.SaveError", "Failed to add question.", 500));

        return Result.Success(new AddQuestionResponse
        {
            QuestionId = question.Id,
            QuizId = question.QuizId,
            Text = question.Text,
            OrderIndex = question.OrderIndex,
            OptionCount = question.Options.Count,
            CreatedAt = question.CreatedAt
        });
    }
}
