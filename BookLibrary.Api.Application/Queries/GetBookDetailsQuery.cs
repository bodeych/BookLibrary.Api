using BookLibrary.Api.Application.Dto;
using BookLibrary.Api.Application.Interfaces;

namespace BookLibrary.Api.Application.Queries;

public sealed class GetBookDetailsQuery : IQuery<BookDetailsDto?>
{
    public required Guid Id { get; init; }
}

internal sealed class GetBookDetailsQueryHandler : IQueryHandler<GetBookDetailsQuery, BookDetailsDto?>
{
    private readonly IBookRepository _bookRepository;

    public GetBookDetailsQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookDetailsDto?> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken);

        if (book is null)
        {
            return null;
        }

        var detailsDto = new BookDetailsDto
        {
            Id = book.Id,
            UserId = book.UserId,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            PublicationYear = book.PublicationYear
        };
        return detailsDto;
    }
    
}