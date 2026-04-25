/* File Overview
 * File: CreateDiplomaCommandHandler.cs
 * Purpose: Application layer (CQRS): defines commands/queries and handlers that implement use-cases through MediatR.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.DTOs.Diplomas;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Interfaces.Repositories;
using Mapster;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Commands.CreateDiploma;

public class CreateDiplomaCommandHandler(IGenericRepository<Diploma> diplomaRepository) : IRequestHandler<CreateDiplomaCommand, Result<GetDiplomaDto>>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository = diplomaRepository;

    public async Task<Result<GetDiplomaDto>> Handle(CreateDiplomaCommand request, CancellationToken cancellationToken)
    {
        var diploma = request.Adapt<Diploma>();

        diploma.Status = DiplomaStatus.Draft;

        await _diplomaRepository.AddAsync(diploma, cancellationToken);
        await _diplomaRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(diploma.Adapt<GetDiplomaDto>());
    }
}
