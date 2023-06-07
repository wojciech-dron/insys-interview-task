using MovieLibrary.Data.Entities;
using Riok.Mapperly.Abstractions;

namespace MovieLibrary.Core.Models;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

[Mapper]
public partial class CategoryMapper
{
    public partial CategoryDto Map(Category category);
}


public static class CategoryMapperExtensions
{
    public static CategoryDto MapToDto(this Category category)
    {
        return new CategoryMapper().Map(category);
    }
}