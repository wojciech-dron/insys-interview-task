using FluentValidation;
using MovieLibrary.Core.Commands.Categories;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Tests.Categories;

public class CreateCategoryTests
{
    [Fact]
    public async Task CreateCategoryCommand_Should_Create_Valid_Category()
    {
        // Arrange
        var mockRepository = Substitute.For<ICategoryRepository>();
        mockRepository.IsNameUniqueAsync("CategoryName", CancellationToken.None)
            .Returns(Task.FromResult(true));
        var validator = new CreateCategoryValidator(mockRepository);
        var handler = new CreateCategoryHandler(validator, mockRepository);

        var command = new CreateCategoryCommand { Name = "CategoryName" };

        // Act
        var category = await handler.Handle(command, CancellationToken.None);

        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be("CategoryName");

        await mockRepository.Received()
            .AddAsync(Arg.Is<Category>(x => x.Name == "CategoryName"), CancellationToken.None);
    }

    [Fact]
    public async Task CreateCategoryCommand_Should_Throw_Exception_If_Category_Name_Is_Not_Unique()
    {
        // Arrange
        var mockRepository = Substitute.For<ICategoryRepository>();
        mockRepository.IsNameUniqueAsync("CategoryName", CancellationToken.None)
            .Returns(Task.FromResult(false));
        var validator = new CreateCategoryValidator(mockRepository);
        var handler = new CreateCategoryHandler(validator, mockRepository);

        var command = new CreateCategoryCommand { Name = "CategoryName" };

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}