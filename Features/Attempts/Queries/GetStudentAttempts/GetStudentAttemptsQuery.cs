/* File Overview
 * File: GetStudentAttemptsQuery.cs
 * Purpose: Application layer (CQRS): defines commands/queries and handlers that implement use-cases through MediatR.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Attempts;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.Queries.GetStudentAttempts;

public record GetStudentAttemptsQuery(
    Guid StudentId,
    int PageNumber,
    int PageSize,
    Guid? QuizId,
    Guid? DiplomaId) : IRequest<PaginatedList<StudentAttemptItemDto>>;

public class GetStudentAttemptsQueryHandler(ApplicationDbContext context)
    : IRequestHandler<GetStudentAttemptsQuery, PaginatedList<StudentAttemptItemDto>>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<PaginatedList<StudentAttemptItemDto>> Handle(GetStudentAttemptsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.QuizAttempts
            .AsNoTracking()
            .Where(x => x.StudentId == request.StudentId)
            .Select(x => new StudentAttemptItemDto(
                x.Id,
                x.QuizId,
                x.Quiz.DiplomaId,
                x.Quiz.Title,
                x.Status,
                x.StartTime,
                x.Deadline,
                x.SubmittedAt,
                x.Score,
                x.Passed));

        if (request.QuizId.HasValue)
        {
            query = query.Where(x => x.QuizId == request.QuizId.Value);
        }

        if (request.DiplomaId.HasValue)
        {
            query = query.Where(x => x.DiplomaId == request.DiplomaId.Value);
        }

        query = query.OrderByDescending(x => x.StartTime);

        return await PaginatedList<StudentAttemptItemDto>
            .CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}

