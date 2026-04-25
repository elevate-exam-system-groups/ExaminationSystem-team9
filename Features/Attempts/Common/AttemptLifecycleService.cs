/* File Overview
 * File: AttemptLifecycleService.cs
 * Purpose: Application layer (CQRS): defines commands/queries and handlers that implement use-cases through MediatR.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Domain.DTOs.Attempts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.Common;

public class AttemptLifecycleService(ApplicationDbContext context) : IAttemptLifecycleService
{
    private readonly ApplicationDbContext _context = context;

    public bool IsExpired(QuizAttempt attempt) => attempt.Deadline <= DateTime.UtcNow;

    public async Task<SubmitAttemptResponse> FinalizeAttemptAsync(QuizAttempt attempt, bool timedOut, CancellationToken cancellationToken)
    {
        if (attempt.Status is QuizAttemptStatus.Submitted or QuizAttemptStatus.TimedOut)
        {
            var existingResult = await _context.QuizAttemptResults
                .AsNoTracking()
                .FirstAsync(x => x.QuizAttemptId == attempt.Id, cancellationToken);

            return new SubmitAttemptResponse(
                attempt.Id,
                attempt.Status,
                existingResult.Score,
                existingResult.Passed,
                existingResult.CorrectAnswers,
                existingResult.TotalQuestions);
        }

        var quiz = await _context.Quizzes
            .Include(x => x.Questions)
                .ThenInclude(x => x.Options)
            .FirstAsync(x => x.Id == attempt.QuizId, cancellationToken);

        var answers = await _context.AttemptAnswers
            .Where(x => x.QuizAttemptId == attempt.Id)
            .ToListAsync(cancellationToken);

        var totalQuestions = quiz.Questions.Count;
        var correctAnswers = 0;

        foreach (var question in quiz.Questions)
        {
            var answer = answers.FirstOrDefault(x => x.QuestionId == question.Id);
            var correctOptionId = question.Options.FirstOrDefault(x => x.IsCorrect)?.Id;
            var isCorrect = answer is not null && answer.SelectedOptionId.HasValue && answer.SelectedOptionId == correctOptionId;

            if (answer is not null)
            {
                answer.IsCorrect = isCorrect;
                answer.UpdatedAt = DateTime.UtcNow;
            }

            if (isCorrect)
            {
                correctAnswers++;
            }
        }

        var score = totalQuestions == 0 ? 0m : Math.Round((decimal)correctAnswers / totalQuestions * 100m, 2);
        var passed = score >= (decimal)quiz.PassScore;

        attempt.Status = timedOut ? QuizAttemptStatus.TimedOut : QuizAttemptStatus.Submitted;
        attempt.SubmittedAt = DateTime.UtcNow;
        attempt.Score = score;
        attempt.Passed = passed;
        attempt.UpdatedAt = DateTime.UtcNow;

        var attemptResult = await _context.QuizAttemptResults.FirstOrDefaultAsync(x => x.QuizAttemptId == attempt.Id, cancellationToken);
        if (attemptResult is null)
        {
            attemptResult = new QuizAttemptResult
            {
                QuizAttemptId = attempt.Id,
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctAnswers,
                Score = score,
                Passed = passed,
                FinalizedAt = DateTime.UtcNow
            };

            await _context.QuizAttemptResults.AddAsync(attemptResult, cancellationToken);
        }
        else
        {
            attemptResult.TotalQuestions = totalQuestions;
            attemptResult.CorrectAnswers = correctAnswers;
            attemptResult.Score = score;
            attemptResult.Passed = passed;
            attemptResult.FinalizedAt = DateTime.UtcNow;
            attemptResult.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new SubmitAttemptResponse(
            attempt.Id,
            attempt.Status,
            score,
            passed,
            correctAnswers,
            totalQuestions);
    }
}

