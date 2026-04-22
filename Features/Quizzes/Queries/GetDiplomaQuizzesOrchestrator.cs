namespace ExaminationSystem.Features.Quizzes.Queries;

public record GetDiplomaQuizzesOrchestrator(Guid DiplomaId) : IRequest<Result<IEnumerable<QuizResponse>>>;

public class GetDiplomaQuizzesOrchestratorHandler(ISender sender) : IRequestHandler<GetDiplomaQuizzesOrchestrator, Result<IEnumerable<QuizResponse>>>
{
    private readonly ISender _sender = sender;

    public async Task<Result<IEnumerable<QuizResponse>>> Handle(GetDiplomaQuizzesOrchestrator request, CancellationToken cancellationToken)
    {
        var isDiplomaExists = await _sender.Send(new GetDiplomaIdIsExistsQuery(request.DiplomaId), cancellationToken);

        if (!isDiplomaExists)
            return Result.Failure<IEnumerable<QuizResponse>>(DiplomaErrors.NotFound(request.DiplomaId));

        var diplomaQuizzes = await _sender.Send(new GetDiplomaQuizzesQuery(request.DiplomaId), cancellationToken);

        return Result.Success(diplomaQuizzes);
    }
}