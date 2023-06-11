using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MovieLibrary.Core.Exceptions;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Commands.Movies;

public class DeleteMovieCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

internal class DeleteMovieHandler : IRequestHandler<DeleteMovieCommand, Unit>
{
    private readonly IMovieRepository _repository;

    public DeleteMovieHandler(IMovieRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteMovieCommand request,
        CancellationToken cancellationToken)
    {
        var movie = await _repository.GetAsync(request.Id, cancellationToken);
        
        if (movie is null)
            throw new NotFoundException(typeof(Movie), request.Id);
        
        await _repository.DeleteAsync(movie, cancellationToken);

        return Unit.Value;
    }
}