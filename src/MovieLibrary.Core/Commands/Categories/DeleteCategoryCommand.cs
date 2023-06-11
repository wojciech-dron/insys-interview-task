using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MovieLibrary.Core.Exceptions;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Commands.Categories;

public class DeleteCategoryCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Unit>
{
    private readonly ICategoryRepository _repository;

    public DeleteCategoryHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await _repository.GetAsync(request.Id);
        
        if (category is null)
            throw new NotFoundException(typeof(Category), request.Id);
        
        await _repository.DeleteAsync(category);

        return Unit.Value;
    }
}