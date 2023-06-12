using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Commands.Categories;

public class CreateCategoryCommand : IRequest<Category>
{
    public string Name { get; set; }
}

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly ICategoryRepository _repository;

    public CreateCategoryValidator(ICategoryRepository repository)
    {
        _repository = repository;
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MustAsync((name, ct) => _repository.IsNameUniqueAsync(name, 0, ct))
            .WithMessage("Category name must be unique.");
    }
}

internal class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Category>
{
    private readonly IValidator<CreateCategoryCommand> _validator;
    private readonly ICategoryRepository _repository;

    public CreateCategoryHandler(IValidator<CreateCategoryCommand> validator,
        ICategoryRepository repository)
    {
        _validator = validator;
        _repository = repository;
    }

    public async Task<Category> Handle(CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        var category = new Category
        {
            Name = request.Name
        };

        await _repository.AddAsync(category, cancellationToken);

        return category;
    }
}