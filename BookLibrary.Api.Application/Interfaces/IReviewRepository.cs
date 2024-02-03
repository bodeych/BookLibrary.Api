using BookLibrary.Api.Domain.Entities;

namespace BookLibrary.Api.Application.Interfaces;

public interface IReviewRepository
{
    Task<IEnumerable<Review>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Review review, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}