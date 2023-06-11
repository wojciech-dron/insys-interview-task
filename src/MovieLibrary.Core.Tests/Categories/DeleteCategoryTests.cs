using MovieLibrary.Core.Commands.Categories;
using MovieLibrary.Core.Exceptions;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Tests.Categories;

public class DeleteCategoryTests
{
    [Fact]
    public async Task DeleteCategoryCommand_Should_Delete_Category()
    {
        // Arrange
        var mockRepository = Substitute.For<ICategoryRepository>();
        var existingCategory = new Category { Id = 1, Name = "CategoryName" };
        mockRepository.GetAsync(1, CancellationToken.None).Returns(Task.FromResult(existingCategory));

        var handler = new DeleteCategoryHandler(mockRepository);

        var command = new DeleteCategoryCommand { Id = 1 };

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await mockRepository.Received().DeleteAsync(Arg.Is<Category>(x => x.Id == 1), CancellationToken.None);
    }

    [Fact]
    public async Task DeleteCategoryCommand_Should_Throw_Exception_If_Category_Not_Found()
    {
        // Arrange
        var mockRepository = Substitute.For<ICategoryRepository>();
        mockRepository.GetAsync(1, CancellationToken.None).Returns(Task.FromResult((Category)null));

        var handler = new DeleteCategoryHandler(mockRepository);

        var command = new DeleteCategoryCommand { Id = 1 };

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}