using AutoMapper;
using FluentValidation;
using MovieLibrary.Core.Commands.Movies;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Tests.Movies;

public class CreateMovieTests
{
    private readonly IMapper _mapper;

    public CreateMovieTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CreateMovieCommandProfile());
        });
        
        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task CreateMovieCommand_Should_Create_Valid_Movie()
    {
        // Arrange
        var mockMovieRepository = Substitute.For<IMovieRepository>();
        var mockCategoryRepository = Substitute.For<ICategoryRepository>();
        mockCategoryRepository.ExistsAsync(Arg.Any<List<int>>(), CancellationToken.None).Returns(Task.FromResult(true));
        
        var validator = new CreateMovieValidator(mockCategoryRepository);
        var handler = new CreateMovieHandler(validator, mockMovieRepository, mockCategoryRepository, _mapper);

        var command = new CreateMovieCommand { 
            Title = "MovieTitle",
            Description = "MovieDescription",
            Year = 2000,
            ImdbRating = 7.2m,
            Categories = new List<int> { 1, 2 }
        };

        // Act
        var movie = await handler.Handle(command, CancellationToken.None);

        // Assert
        movie.Should().NotBeNull();
        movie.Title.Should().Be("MovieTitle");
        movie.Description.Should().Be("MovieDescription");
        movie.Year.Should().Be(2000);
        movie.ImdbRating.Should().Be(7.2m);
        movie.MovieCategories.Should().HaveCount(2);

        await mockMovieRepository.Received().AddAsync(Arg.Is<Movie>(x => 
                x.Title == "MovieTitle" && 
                x.Description == "MovieDescription" && 
                x.Year == 2000 && 
                x.ImdbRating == 7.2m && 
                x.MovieCategories.Count == 2), 
            CancellationToken.None);
    }

    [Fact]
    public async Task CreateMovieCommand_Should_Throw_Exception_If_Category_Does_Not_Exist()
    {
        // Arrange
        var mockMovieRepository = Substitute.For<IMovieRepository>();
        var mockCategoryRepository = Substitute.For<ICategoryRepository>();
        mockCategoryRepository.ExistsAsync(Arg.Any<List<int>>(), CancellationToken.None).Returns(Task.FromResult(false));

        var validator = new CreateMovieValidator(mockCategoryRepository);
        var handler = new CreateMovieHandler(validator, mockMovieRepository, mockCategoryRepository, _mapper);

        var command = new CreateMovieCommand { 
            Title = "MovieTitle",
            Description = "MovieDescription",
            Year = 2000,
            ImdbRating = 7.2m,
            Categories = new List<int> { 1, 2 }
        };

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}