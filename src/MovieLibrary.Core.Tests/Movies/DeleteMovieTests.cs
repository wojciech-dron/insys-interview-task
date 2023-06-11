using MediatR;
using MovieLibrary.Core.Commands.Movies;
using MovieLibrary.Core.Exceptions;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Tests.Movies;

public class DeleteMovieTests
{
    [Fact]
    public async Task DeleteMovieCommand_Should_Delete_Existing_Movie()
    {
        // Arrange
        var mockRepository = Substitute.For<IMovieRepository>();
        mockRepository.GetAsync(1, CancellationToken.None).Returns(Task.FromResult(new Movie { Id = 1 }));

        var handler = new DeleteMovieHandler(mockRepository);

        var command = new DeleteMovieCommand { Id = 1 };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        await mockRepository.Received().DeleteAsync(Arg.Is<Movie>(x => x.Id == 1), CancellationToken.None);
    }

    [Fact]
    public async Task DeleteMovieCommand_Should_Throw_Exception_If_Movie_Does_Not_Exist()
    {
        // Arrange
        var mockRepository = Substitute.For<IMovieRepository>();

        var handler = new DeleteMovieHandler(mockRepository);

        var command = new DeleteMovieCommand { Id = 1 };

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}