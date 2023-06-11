using System.Data;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MovieLibrary.Core.Exceptions;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Commands.Categories;

public class UpdateCategoryCommand : IRequest<Category>
{
    [JsonIgnore] public int Id { get; set; }
    public string Name { get; set; }
}

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _repository;

    public UpdateCategoryValidator(ICategoryRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Id).GreaterThan(0);
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MustAsync(UniqueAsync)
            .WithMessage("Category name must be unique.");
    }

    private async Task<bool> UniqueAsync(string name, CancellationToken cancellationToken)
    {
        return await _repository.IsNameUniqueAsync(name);
    }
}

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Category>
{
    private readonly ICategoryRepository _repository;
    private readonly IValidator<UpdateCategoryCommand> _validator;

    public UpdateCategoryHandler(ICategoryRepository repository,
        IValidator<UpdateCategoryCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Category> Handle(UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var category = await _repository.GetAsync(request.Id);
        
        if (category is null)
            throw new NotFoundException(typeof(Category), request.Id);
        
        category.Name = request.Name;
        
        await _repository.UpdateAsync(category);
        return category;
    }
}