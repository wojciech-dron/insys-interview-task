using System.Collections.Generic;
using System.Linq;
using MovieLibrary.Core.Utils.Pagination;
using MovieLibrary.Data.Entities;

namespace MovieLibrary.Core.Queries.Filters;

public class MovieFilter : Pager
{
    public string TitlePhrase { get; set; }
    public List<int> Categories { get; set; }
    public decimal? ImdbRatingMin { get; set; }
    public decimal? ImdbRatingMax { get; set; }

    public MovieFilter()
    {
        // SQLite does not support expressions of type 'decimal' in ORDER BY clauses.
        // OrderBy = nameof(Movie.ImdbRating);
        // Direction = "DESC";
    }

    public IQueryable<Movie> Filter(IQueryable<Movie> query)
    {
        if (!string.IsNullOrWhiteSpace(TitlePhrase)) 
            query = query.Where(x => x.Title.Contains(TitlePhrase));
        
        if (Categories?.Any() == true)
            query = query.Where(x => x.MovieCategories.Any(c => Categories.Contains(c.CategoryId)));
        
        if (ImdbRatingMin.HasValue)
            query = query.Where(x => x.ImdbRating >= ImdbRatingMin.Value);
        
        if (ImdbRatingMax.HasValue)
            query = query.Where(x => x.ImdbRating <= ImdbRatingMax.Value);

        return query;
    }
}

public static class MovieFilterExtensions
{
    public static IQueryable<Movie> Filter(this IQueryable<Movie> query, MovieFilter filter)
    {
        return filter.Filter(query);
    }
}