using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using MovieLibrary.Data.Entities;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Core.Commands.Movies;

public class CreateMovieCommand : IRequest<Movie>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int Year { get; set; }
    public decimal? ImdbRating { get; set; }
    public List<int> Categories { get; set; } = new();
}

public class CreateMovieValidator : AbstractValidator<CreateMovieCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateMovieValidator(ICategoryRepository categoryRepository)
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

internal class CreateMovieHandler : IRequestHandler<CreateMovieCommand, Movie>
{
    private readonly IValidator<CreateMovieCommand> _validator;
    private readonly IMovieRepository _movieRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CreateMovieHandler(IValidator<CreateMovieCommand> validator,
        IMovieRepository movieRepository,
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        _validator = validator;
        _movieRepository = movieRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Movie> Handle(CreateMovieCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        await _categoryRepository.GetAsync(request.Categories, cancellationToken); // for proper categories mapping
        var movie = _mapper.Map<Movie>(request);
        
        await _movieRepository.AddAsync(movie, cancellationToken);

        return movie;
    }
}

internal class CreateMovieCommandProfile : Profile
{
    public CreateMovieCommandProfile()
    {
        CreateMap<CreateMovieCommand, Movie>()
            .ForMember(dst => dst.Id , opt => opt.Ignore())
            .ForMember(dst => dst.MovieCategories, opt => opt.MapFrom(src =>
                src.Categories.ConvertAll(categoryId => new MovieCategory { CategoryId = categoryId })));
    }
}