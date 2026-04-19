using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.QuizResponse;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Interfaces.Repositories;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Commands.CreateQuiz
{

    // Location: Application/Features/Quizzes/Commands/CreateQuiz/CreateQuizCommandHandler.cs
    public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, Result<QuizResponse>>
    {
        private readonly IGenericRepository<Quiz> _quizRepository;

        public CreateQuizCommandHandler(IGenericRepository<Quiz> quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<Result<QuizResponse>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            // 1. Map Command to Entity (يدوي أو بـ Mapster)
            var quiz = new Quiz
            {
                Id = Guid.NewGuid(),
                DiplomaId = request.DiplomaId,
                Title = request.Title,
                Instructions = request.Instructions,
                DurationMinutes = request.DurationMinutes,
                PassScore = request.PassScore,
                MaxAttempts = request.MaxAttempts,
                Status = QuizStatus.Draft, // القيمة الافتراضية
                CreatedAt = DateTime.UtcNow
            };

            // 2. Save
            await _quizRepository.AddAsync(quiz, cancellationToken);
            var result = await _quizRepository.SaveChangesAsync(cancellationToken);

            if (result <= 0)
                return (Result<QuizResponse>)Result<QuizResponse>.Failure(new Error("Quiz.SaveError", "Failed to save quiz.", null));

            // 3. Map Entity to Response
            var response = new QuizResponse
            {
                QuizId = quiz.Id,
                DiplomaId = quiz.DiplomaId,
                Title = quiz.Title,
                DurationMinutes = quiz.DurationMinutes,
                PassScore = quiz.PassScore,
                MaxAttempts = quiz.MaxAttempts,
                Status = quiz.Status.ToString(),
                QuestionCount = 0,
                CreatedAt = quiz.CreatedAt
            };

            return Result<QuizResponse>.Success(response);
        }
    }
}
