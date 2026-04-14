using System.Linq.Expressions;

namespace ExaminationSystem.Domain.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> GetQueryable();
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void SoftDelete(T entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
