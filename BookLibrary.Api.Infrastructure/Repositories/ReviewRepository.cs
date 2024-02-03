using BookLibrary.Api.Application.Interfaces;
using BookLibrary.Api.Domain.Entities;
using BookLibrary.Api.Infrastructure.Settings;
using Npgsql;

namespace BookLibrary.Api.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly DatabaseSettings _databaseSettings;

    public ReviewRepository(DatabaseSettings databaseSettings)
    {
        _databaseSettings = databaseSettings;
    }
    
    public async Task AddAsync(Review review, CancellationToken cancellationToken)
    {
        using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);
            using (var command = new NpgsqlCommand("INSERT INTO reviews (id, book_id, user_id, comment, rating) VALUES (@id, @book_id, @user_id, @comment, @rating)", connection))
            {
                command.Parameters.AddWithValue("@id", review.Id);
                command.Parameters.AddWithValue("@book_id", review.BookId);
                command.Parameters.AddWithValue("@user_id", review.UserId);
                command.Parameters.AddWithValue("@comment", review.Comment);
                command.Parameters.AddWithValue("@rating", review.Rating);

                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<Review>> IReviewRepository.GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}