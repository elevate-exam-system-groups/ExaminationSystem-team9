using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Interfaces.Repositories;
using ExaminationSystem.DTOs.Questions;
using ExaminationSystem.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.Commands.UpdateQuestion;

public class UpdateQuestionCommandHandler(IGenericRepository<Question> questionRepository)
    : IRequestHandler<UpdateQuestionCommand, Result<UpdateQuestionResponse>>
{
    private readonly IGenericRepository<Question> _questionRepository = questionRepository;

    public async Task<Result<UpdateQuestionResponse>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository
            .GetQueryable()
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == request.QuestionId, cancellationToken);

        if (question is null)
            return Result.Failure<UpdateQuestionResponse>(QuestionError.NotFound(request.QuestionId));

        var existingOptionIds = question.Options
            .Select(option => option.Id)
            .OrderBy(id => id)
            .ToList();

        var requestOptionIds = request.Options
            .Select(option => option.OptionId)
            .OrderBy(id => id)
            .ToList();

        if (existingOptionIds.Count != requestOptionIds.Count || !existingOptionIds.SequenceEqual(requestOptionIds))
            return Result.Failure<UpdateQuestionResponse>(QuestionError.InvalidOptions(request.QuestionId));

        question.Text = request.Text;
        question.OrderIndex = request.OrderIndex;
        question.Explanation = request.Explanation;
        question.UpdatedAt = DateTime.UtcNow;

        foreach (var option in question.Options)
        {
            var requestOption = request.Options.First(x => x.OptionId == option.Id);
            option.Text = requestOption.Text;
            option.IsCorrect = requestOption.IsCorrect;
            option.UpdatedAt = DateTime.UtcNow;
        }

        _questionRepository.Update(question);
        await _questionRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateQuestionResponse
        {
            QuestionId = question.Id,
            UpdatedAt = question.UpdatedAt!.Value
        });
    }
}
