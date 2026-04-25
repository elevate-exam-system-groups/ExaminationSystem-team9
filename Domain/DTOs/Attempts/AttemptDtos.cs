/* File Overview
 * File: AttemptDtos.cs
 * Purpose: Supporting application source file within the Clean Architecture solution.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.DTOs.Attempts;

public record StartQuizResponse(
    Guid AttemptId,
    Guid QuizId,
    DateTime StartTime,
    DateTime Deadline,
    List<StartQuestionDto> Questions);

public record StartQuestionDto(Guid QuestionId, string Text, List<StartOptionDto> Options);
public record StartOptionDto(Guid OptionId, string Text);

public record SubmitAnswerRequest(Guid QuestionId, Guid SelectedOptionId);
public record TimerResponse(Guid AttemptId, int SecondsRemaining);

public record SubmitAttemptResponse(
    Guid AttemptId,
    QuizAttemptStatus Status,
    decimal Score,
    bool Passed,
    int CorrectAnswers,
    int TotalQuestions);

public record AttemptResultQuestionBreakdownDto(
    Guid QuestionId,
    string QuestionText,
    Guid? SelectedOptionId,
    string? SelectedOptionText,
    Guid? CorrectOptionId,
    string? CorrectOptionText,
    bool IsCorrect,
    string? Explanation);

public record AttemptResultDto(
    Guid AttemptId,
    Guid QuizId,
    QuizAttemptStatus Status,
    decimal Score,
    bool Passed,
    int CorrectAnswers,
    int TotalQuestions,
    List<AttemptResultQuestionBreakdownDto> Questions);

public record StudentAttemptItemDto(
    Guid AttemptId,
    Guid QuizId,
    Guid DiplomaId,
    string QuizTitle,
    QuizAttemptStatus Status,
    DateTime StartTime,
    DateTime Deadline,
    DateTime? SubmittedAt,
    decimal? Score,
    bool? Passed);

