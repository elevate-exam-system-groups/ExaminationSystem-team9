using ExaminationSystem.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Interfaces.Repositories;
using ExaminationSystem.Errors;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Commands.UpdateDiploma;

public class UpdateDiplomaCommandHandler(IGenericRepository<Diploma> diplomaRepository) : IRequestHandler<UpdateDiplomaCommand, Result>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository = diplomaRepository;

    public async Task<Result> Handle(UpdateDiplomaCommand request, CancellationToken cancellationToken)
    {
        var diploma = await _diplomaRepository.GetByIdAsync(request.Id, cancellationToken);

        if (diploma is null)
            return Result.Failure(DiplomaError.NotFound(request.Id));

        diploma.Title = request.Title;
        diploma.Description = request.Description;
        diploma.UpdatedAt = DateTime.UtcNow;

        await _diplomaRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
