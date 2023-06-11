using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieLibrary.Data.Repositories.Abstract;

public interface IRepository<TEntity>
{
    public IQueryable<TEntity> GetQuery();
    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    public Task<TEntity> GetAsync(int id, CancellationToken cancellationToken = default);
    public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}