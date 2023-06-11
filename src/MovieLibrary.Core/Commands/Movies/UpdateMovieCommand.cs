using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using MovieLibrary.Core.Exceptions;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Commands.Movies;

public class UpdateMovieCommand : IRequest<Movie>
{
    [JsonIgnore] public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Year { get; set; }
    public decimal? ImdbRating { get; set; }
    public List<int> Categories { get; set; } = new();
}

public class UpdateMovieValidator : AbstractValidator<UpdateMovieCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateMovieValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Year).GreaterThan(1900);
        RuleFor(x => x.ImdbRating)
            .NotNull()
            .LessThanOrEqualTo(10)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Categories)
            .NotEmpty()
            .MustAsync(ExistsAsync);
    }

    private Task<bool> ExistsAsync(List<int> ids, CancellationToken cancellationToken)
    {
        return _categoryRepository.ExistsAsync(ids, cancellationToken);
    }
}

internal class UpdateMovieHandler : IRequestHandler<UpdateMovieCommand, Movie>
{
    private readonly IMovieRepository _movieRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<UpdateMovieCommand> _validator;
    private readonly IMapper _mapper;

    public UpdateMovieHandler(IMovieRepository movieRepository,
        ICategoryRepository categoryRepository,
        IValidator<UpdateMovieCommand> validator,
        IMapper mapper)
    {
        _movieRepository = movieRepository;
        _categoryRepository = categoryRepository;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<Movie> Handle(UpdateMovieCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var movie = await _movieRepository.GetAsync(request.Id, cancellationToken);
        if (movie is null)
            throw new NotFoundException(typeof(Movie), request.Id);

        await _categoryRepository.GetAsync(request.Categories, cancellationToken); // for proper categories mapping
        _mapper.Map(request, movie);

        await _movieRepository.UpdateAsync(movie, cancellationToken);
        
        return movie;
    }
}

internal class UpdateMovieProfile : Profile
{
    public UpdateMovieProfile()
    {
        CreateMap<UpdateMovieCommand, Movie>()
            .ForMember(dst => dst.MovieCategories, opt => opt.MapFrom(src => src.Categories
                .ConvertAll(categoryId => new MovieCategory { CategoryId = categoryId })));
    }
}