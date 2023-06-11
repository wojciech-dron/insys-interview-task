using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MovieLibrary.Data.Repositories.Abstract;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly MovieLibraryContext Context;
    protected readonly DbSet<TEntity> DbSet;

    protected Repository(MovieLibraryContext context)
    {
        Context = context;
        DbSet = Context.Set<TEntity>();
    }
    
    public virtual IQueryable<TEntity> GetQuery()
    {
        return DbSet.AsQueryable();
    }

    public virtual async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(new object[] { id }, cancellationToken) is not null;
    }

    public virtual async Task<TEntity> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        
        return entity;
    }
    
    public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        await SaveChangesAsync(cancellationToken);

        return entity;
    }
    
    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        
        await SaveChangesAsync(cancellationToken);
    }
    
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await Context.SaveChangesAsync(cancellationToken);
    }
}