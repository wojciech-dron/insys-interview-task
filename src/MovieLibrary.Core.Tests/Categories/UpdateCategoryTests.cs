using FluentValidation;
using MovieLibrary.Core.Commands.Categories;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Tests.Categories;

public class UpdateCategoryTests
{
    [Fact]
    public async Task UpdateCategoryCommand_Should_Update_Category()
    {
        // Arrange
        var mockRepository = Substitute.For<ICategoryRepository>();
        var existingCategory = new Category { Id = 1, Name = "OldCategoryName" };
        mockRepository.GetAsync(1, CancellationToken.None).Returns(Task.FromResult(existingCategory));
        mockRepository.IsNameUniqueAsync("NewCategoryName").Returns(Task.FromResult(true));

        var validator = new UpdateCategoryValidator(mockRepository);
        var handler = new UpdateCategoryHandler(mockRepository, validator);

        var command = new UpdateCategoryCommand { Id = 1, Name = "NewCategoryName" };

        // Act
        var category = await handler.Handle(command, CancellationToken.None);

        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be("NewCategoryName");

        await mockRepository.Received().UpdateAsync(Arg.Is<Category>(x => x.Name == "NewCategoryName" && x.Id == 1), CancellationToken.None);
    }

    [Fact]
    public async Task UpdateCategoryCommand_Should_Throw_Exception_If_Category_Name_Is_Not_Unique()
    {
        // Arrange
        var mockRepository = Substitute.For<ICategoryRepository>();
        var existingCategory = new Category { Id = 1, Name = "OldCategoryName" };
        mockRepository.GetAsync(1, CancellationToken.None).Returns(Task.FromResult(existingCategory));
        mockRepository.IsNameUniqueAsync("NonUniqueCategoryName").Returns(Task.FromResult(false));

        var validator = new UpdateCategoryValidator(mockRepository);
        var handler = new UpdateCategoryHandler(mockRepository, validator);

        var command = new UpdateCategoryCommand { Id = 1, Name = "NonUniqueCategoryName" };

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task UpdateCategoryCommand_Should_Throw_Exception_If_Category_Not_Found()
    {
        // Arrange
        var mockRepository = Substitute.For<ICategoryRepository>();
        mockRepository.GetAsync(1, CancellationToken.None).Returns(Task.FromResult((Category)null!));

        var validator = new UpdateCategoryValidator(mockRepository);  
        var handler = new UpdateCategoryHandler(mockRepository, validator); 

        var command = new UpdateCategoryCommand { Id = 1, Name = "NonExistentCategory" };

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}