using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Commands.Categories;


public class CreateCategory
{
    public class Command : IRequest<Category>
    {
        public string Name { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3);
        }
    }

    public class Handler : IRequestHandler<Command, Category>
    {
        private readonly ICategoryRepository _repository;

        public Handler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Category> Handle(Command request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = request.Name
            };

            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();

            return category;
        }
    }
}