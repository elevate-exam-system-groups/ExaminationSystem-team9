using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Interfaces.Repositories;
using ExaminationSystem.Infrastructure.Implementations.Repositories;
using MediatR;
using Org.BouncyCastle.Security;

namespace ExaminationSystem.Features.Quizzes.Commands.UpdateQuiz
{
    public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, Result>
    {
        private readonly IGenericRepository<Quiz> _repository;
        public UpdateQuizCommandHandler(IGenericRepository<Quiz> genericRepository)
        {
            _repository = genericRepository;
        }

        public async Task<Result> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
        {
            // data validation
            var quiz = await _repository.GetByIdAsync(request.Id);

                if (quiz == null)
                    return Result.Failure<Guid>(new Error("no found","not found",null));
                    
            // edit and update
            quiz.Title = request.Title;
            quiz.Instructions = request.Instructions;
            quiz.DurationMinutes = request.DurationMinutes;
            quiz.PassScore = request.PassScore;
            quiz.MaxAttempts = request.MaxAttempts;

            _repository.Update(quiz);
            await _repository.SaveChangesAsync();

            return Result.Success(quiz.Id);

        }
    }
}
