using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories.Abstract;

namespace MovieLibrary.Data.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetAsync(List<int> categoriesIds, CancellationToken cancellationToken = default);
    Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}