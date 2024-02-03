using System.Data;
using BookLibrary.Api.Application.Interfaces;
using BookLibrary.Api.Domain.Entities;
using BookLibrary.Api.Infrastructure.Settings;
using Npgsql;

namespace BookLibrary.Api.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly DatabaseSettings _databaseSettings;

    public BookRepository(DatabaseSettings databaseSettings)
    {
        _databaseSettings = databaseSettings;
    }

    public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken)
    {
        using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);
            using (var command = new NpgsqlCommand("SELECT * FROM books", connection))
            {
                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    var books = new List<Book>();

                    while (await reader.ReadAsync(cancellationToken))
                    {
                        books.Add(MapToBook(reader));
                    }

                    return books;
                }
            }
        }
    }

    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);
            using (var command = new NpgsqlCommand("SELECT * FROM books WHERE id = @id", connection))
            {
                command.Parameters.AddWithValue("@id", id);
                
                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        return MapToBook(reader);
                    }

                    return null;
                }
            }
        }
    }
    
    public async Task AddAsync(Book book, CancellationToken cancellationToken)
    {
        using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);
            using (var command = new NpgsqlCommand("INSERT INTO books (id, user_id, title, author, genre, publication_year) VALUES (@id, @user_id, @title, @author, @genre, @publication_year)", connection))
            {
                command.Parameters.AddWithValue("@id", book.Id);
                command.Parameters.AddWithValue("@user_id", book.UserId);
                command.Parameters.AddWithValue("@title", book.Title);
                command.Parameters.AddWithValue("@author", book.Author);
                command.Parameters.AddWithValue("@genre", book.Genre);
                command.Parameters.AddWithValue("@publication_year", book.PublicationYear);

                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }
    
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);
            using (var command = new NpgsqlCommand("DELETE FROM books WHERE id = @id", connection))
            {
               command.Parameters.AddWithValue("@id", id);

                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }
    
    private Book MapToBook(IDataReader reader)
    {
        return Book.CreateFromDatabase(
            reader.GetGuid(reader.GetOrdinal("id")),
            reader.GetGuid(reader.GetOrdinal("user_id")),
            reader.GetString(reader.GetOrdinal("title")),
            reader.GetString(reader.GetOrdinal("author")),
            reader.GetString(reader.GetOrdinal("genre")),
            reader.GetInt32(reader.GetOrdinal("publication_year"))
        );
    }
}