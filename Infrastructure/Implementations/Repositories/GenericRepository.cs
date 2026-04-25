/* File Overview
 * File: GenericRepository.cs
 * Purpose: Infrastructure services: concrete implementations for domain/application abstractions.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using ExaminationSystem.Domain.Interfaces.Repositories;
using ExaminationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExaminationSystem.Infrastructure.Implementations.Repositories;

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context = context;

    public IQueryable<T> GetQueryable() => _context.Set<T>().AsNoTracking();
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _context.Set<T>().FindAsync(id, cancellationToken);
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Set<T>().FindAsync(id, cancellationToken);

    public async Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
        await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
        await _context.Set<T>().AddAsync(entity, cancellationToken);

    public void Update(T entity) => _context.Set<T>().Update(entity);

    public void SoftDelete(T entity) => _context.Set<T>().Remove(entity);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}

