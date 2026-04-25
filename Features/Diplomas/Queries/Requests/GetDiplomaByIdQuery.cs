/* File Overview
 * File: GetDiplomaByIdQuery.cs
 * Purpose: Application layer (CQRS): defines commands/queries and handlers that implement use-cases through MediatR.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Diplomas;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Interfaces.Repositories;
using ExaminationSystem.Errors;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Queries.Requests;

public record GetDiplomaByIdQuery(Guid Id) : IRequest<Result<GetDiplomaDto>>;

public class GetDiplomaByIdQueryHandler(IGenericRepository<Diploma> DiplomaRepository) : IRequestHandler<GetDiplomaByIdQuery, Result<GetDiplomaDto>>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository = DiplomaRepository;

    public async Task<Result<GetDiplomaDto>> Handle(GetDiplomaByIdQuery request, CancellationToken cancellationToken)
    {
        var diploma = await _diplomaRepository
        .GetQueryable()
        .Where(c => c.Id == request.Id)
        .ProjectToType<GetDiplomaDto>()
        .FirstOrDefaultAsync(cancellationToken);

        if (diploma is null)
            return Result.Failure<GetDiplomaDto>(DiplomaError.NotFound(request.Id));

        return Result.Success(diploma);
    }
}
