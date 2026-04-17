using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Quizzes.Common;
using ExaminationSystem.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.Commands.CreateQuiz
{

    public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, Result<QuizResponse>>
    {

        private readonly ApplicationDbContext _context;

        public CreateQuizCommandHandler(ApplicationDbContext context)
        {

            _context = context;

        }
        public async Task<Result<QuizResponse>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            var diplomaExtist = await _context.Diplomas.AnyAsync(d => d.Id == request.DiplomaId, cancellationToken);

            if (!diplomaExtist)
            {
                return Result.Failure<QuizResponse>(new Error("Diploma.NotFound", "الـ Diploma دي مش موجودة عندنا", null));
            }

            var quiz = request.Adapt<Quiz>();

            var response = quiz.Adapt<QuizResponse>();

            return Result<QuizResponse>.Success(response);

        }
    }
}
