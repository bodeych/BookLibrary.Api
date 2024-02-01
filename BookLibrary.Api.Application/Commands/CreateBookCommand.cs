using BookLibrary.Api.Application.Interfaces;
using BookLibrary.Api.Domain.Entities;

namespace BookLibrary.Api.Application.Commands;

public sealed class CreateBookCommand : ICommand<Guid>
{
    public required Guid UserId { get; init; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Genre { get; set; }
    public required int PublicationYear { get; set; }
}

internal sealed class CreateBookCommandHandler : ICommandHandler<CreateBookCommand, Guid>
{
    private readonly IBookRepository _bookRepository;

    public CreateBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    public async Task<Guid> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = Book.Create(request.UserId, request.Title, request.Author, request.Genre, request.PublicationYear);
        
        await _bookRepository.AddBook(book, cancellationToken);

        return book.Id;
    }
}