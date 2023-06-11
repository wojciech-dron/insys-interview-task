using AutoMapper;
using MovieLibrary.Data.Entities;

namespace MovieLibrary.Core.Models;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class CategoryDtoProfile : Profile
{
    public CategoryDtoProfile()
    {
        CreateMap<Category, CategoryDto>()
            .ReverseMap();
    }
}