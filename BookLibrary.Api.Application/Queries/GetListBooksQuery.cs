using BookLibrary.Api.Application.Dto;
using BookLibrary.Api.Application.Interfaces;

namespace BookLibrary.Api.Application.Queries;

public class GetListBooksQuery: IQuery<List<BookDetailsDto>>
{
}

internal sealed class GetListOrdersQueryHandler : IQueryHandler<GetListBooksQuery, List<BookDetailsDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetListOrdersQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<BookDetailsDto>> Handle(GetListBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetAllAsync(cancellationToken);

        var detailsDto = books.Select(book => new BookDetailsDto
        {
            Id = book.Id,
            UserId = book.UserId,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            PublicationYear = book.PublicationYear
        }).ToList();
        
        return detailsDto;
    }
}