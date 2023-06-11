using System.Linq;
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
    
    public IQueryable<TEntity> GetQuery()
    {
        return DbSet.AsQueryable();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return (await DbSet.FindAsync(id)) is not null;
    }

    public async Task<TEntity> GetAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await SaveChangesAsync();
        
        return entity;
    }
    
    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        await SaveChangesAsync();

        return entity;
    }
    
    public async Task DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        
        await SaveChangesAsync();
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await Context.SaveChangesAsync();
    }
}