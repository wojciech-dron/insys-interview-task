using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Commands.Categories;


public class UpdateCategory
{
    public class Command : IRequest<Category>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        private readonly ICategoryRepository _repository;

        public Validator(ICategoryRepository repository)
        {
            _repository = repository;
            
            RuleFor(x => x.Id)
                .MustAsync(ExistAsync);
            
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3);
        }

        private async Task<bool> ExistAsync(int id, CancellationToken cancellationToken)
        {
            return await _repository.ExistsAsync(id);
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
            var category = await _repository.GetAsync(request.Id);
            
            category.Name = request.Name;
            
            await _repository.UpdateAsync(category);

            return category;
        }
    }
}