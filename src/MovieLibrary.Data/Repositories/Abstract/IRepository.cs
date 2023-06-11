using System.Linq;
using System.Threading.Tasks;

namespace MovieLibrary.Data.Repositories.Abstract;

public interface IRepository<TEntity>
{
    public IQueryable<TEntity> GetQuery();
    public Task<bool> ExistsAsync(int id);
    public Task<TEntity> GetAsync(int id);
    public Task<TEntity> AddAsync(TEntity entity);
    public Task<TEntity> UpdateAsync(TEntity entity);
    public Task DeleteAsync(TEntity entity);
    public Task<int> SaveChangesAsync();
}