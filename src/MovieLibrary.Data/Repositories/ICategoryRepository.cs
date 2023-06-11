using System.Threading.Tasks;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories.Abstract;

namespace MovieLibrary.Data.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<bool> IsNameUniqueAsync(string name);
}