using BookLibrary.Api.Domain.Entities;

namespace BookLibrary.Api.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<User?> FindByTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
}