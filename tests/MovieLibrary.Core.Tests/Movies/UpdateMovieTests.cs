using AutoMapper;
using FluentValidation;
using MovieLibrary.Core.Commands.Movies;
using MovieLibrary.Core.Exceptions;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Tests.Movies;

public class UpdateMovieTests
{
    private readonly IMapper _mapper;

    public UpdateMovieTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new UpdateMovieProfile());
        });
        
        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task UpdateMovieCommand_Should_Update_Valid_Movie()
    {
        // Arrange
        var mockMovieRepository = Substitute.For<IMovieRepository>();
        var mockCategoryRepository = Substitute.For<ICategoryRepository>();
        mockCategoryRepository.ExistsAsync(Arg.Any<List<int>>(), CancellationToken.None).Returns(Task.FromResult(true));
        mockMovieRepository.GetAsync(1, CancellationToken.None).Returns(Task.FromResult(new Movie { Id = 1 }));

        var validator = new UpdateMovieValidator(mockCategoryRepository);
        var handler = new UpdateMovieHandler(mockMovieRepository, mockCategoryRepository, validator, _mapper);

        var command = new UpdateMovieCommand
        {
            Id = 1,
            Title = "UpdatedMovieTitle",
            Description = "UpdatedMovieDescription",
            Year = 2001,
            ImdbRating = 8.2m,
            Categories = new List<int> { 2, 3 }
        };

        // Act
        var movie = await handler.Handle(command, CancellationToken.None);

        // Assert
        movie.Should().NotBeNull();
        movie.Title.Should().Be("UpdatedMovieTitle");
        movie.Description.Should().Be("UpdatedMovieDescription");
        movie.Year.Should().Be(2001);
        movie.ImdbRating.Should().Be(8.2m);
        movie.MovieCategories.Should().HaveCount(2);

        await mockMovieRepository.Received().UpdateAsync(Arg.Is<Movie>(x =>
                x.Title == "UpdatedMovieTitle" &&
                x.Description == "UpdatedMovieDescription" &&
                x.Year == 2001 &&
                x.ImdbRating == 8.2m &&
                x.MovieCategories.Count == 2),
            CancellationToken.None);
    }

    [Fact]
    public async Task UpdateMovieCommand_Should_Throw_Exception_If_Movie_Does_Not_Exist()
    {
        // Arrange
        var mockMovieRepository = Substitute.For<IMovieRepository>();
        var mockCategoryRepository = Substitute.For<ICategoryRepository>();
        mockCategoryRepository.ExistsAsync(Arg.Any<List<int>>(), CancellationToken.None).Returns(Task.FromResult(true));

        var validator = new UpdateMovieValidator(mockCategoryRepository);
        var handler = new UpdateMovieHandler(mockMovieRepository, mockCategoryRepository, validator, _mapper);

        var command = new UpdateMovieCommand
        {
            Id = 1,
            Title = "UpdatedMovieTitle",
            Description = "UpdatedMovieDescription",
            Year = 2001,
            ImdbRating = 8.2m,
            Categories = new List<int> { 2, 3 }
        };

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateMovieCommand_Should_Throw_Exception_If_Category_Does_Not_Exist()
    {
        // Arrange
        var mockMovieRepository = Substitute.For<IMovieRepository>();
        var mockCategoryRepository = Substitute.For<ICategoryRepository>();
        mockCategoryRepository.ExistsAsync(Arg.Any<List<int>>(), CancellationToken.None).Returns(Task.FromResult(false));
        mockMovieRepository.GetAsync(1, CancellationToken.None).Returns(Task.FromResult(new Movie { Id = 1 }));

        var validator = new UpdateMovieValidator(mockCategoryRepository);
        var handler = new UpdateMovieHandler(mockMovieRepository, mockCategoryRepository, validator, _mapper);

        var command = new UpdateMovieCommand
        {
            Id = 1,
            Title = "UpdatedMovieTitle",
            Description = "UpdatedMovieDescription",
            Year = 2001,
            ImdbRating = 8.2m,
            Categories = new List<int> { 2, 3 }
        };

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}