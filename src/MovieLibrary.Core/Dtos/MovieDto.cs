using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MovieLibrary.Data.Entities;

namespace MovieLibrary.Core.Dtos;

public class MovieDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public int Year { get; set; }

    public decimal ImdbRating { get; set; }
    
    public List<CategoryDto> Categories { get; set; } = new();
}

public class MovieDtoProfile : Profile
{
    public MovieDtoProfile()
    {
        CreateMap<Movie, MovieDto>()
            .ForMember(dest => dest.Categories, opt => 
                opt.MapFrom(src => src.MovieCategories.Select(mc => mc.Category)));
    }
}