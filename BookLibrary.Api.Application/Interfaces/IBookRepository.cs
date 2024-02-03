using BookLibrary.Api.Domain.Entities;

namespace BookLibrary.Api.Application.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken);
    
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task AddAsync(Book book, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}