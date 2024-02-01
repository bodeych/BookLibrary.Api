using BookLibrary.Api.Domain.Entities;

namespace BookLibrary.Api.Application.Interfaces;

public interface IBookRepository
{
    IEnumerable<Book> GetAllBooks();
    
    Book GetBookById(Guid id);
    
    Task AddBook(Book book, CancellationToken cancellationToken);
}