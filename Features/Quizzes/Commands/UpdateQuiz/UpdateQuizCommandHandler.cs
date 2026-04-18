using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Interfaces.Repositories;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Commands.UpdateQuiz
{
    public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, Result<Guid>>
    {
        private readonly IGenericRepository<Quiz> _repository;

        public UpdateQuizCommandHandler(IGenericRepository<Quiz> genericRepository)
        {
            _repository = genericRepository;
        }

        public async Task<Result<Guid>> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (quiz == null)
                return Result.Failure<Guid>(new Error("Quiz.NotFound", "Quiz not found.", 404));

            quiz.Title = request.Title;
            quiz.Instructions = request.Instructions;
            quiz.DurationMinutes = request.DurationMinutes;
            quiz.PassScore = request.PassScore;
            quiz.MaxAttempts = request.MaxAttempts;
            quiz.UpdatedAt = DateTime.UtcNow;

            _repository.Update(quiz);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success(quiz.Id);
        }
    }
}
