using System.Data;
using BookLibrary.Api.Application.Interfaces;
using BookLibrary.Api.Domain.Entities;
using BookLibrary.Api.Infrasrtucture.Settings;
using Npgsql;

namespace BookLibrary.Api.Infrasrtucture.Repositories;

public class BookRepository : IBookRepository
{
    private readonly DatabaseSettings _databaseSettings;
    
    public BookRepository(DatabaseSettings databaseSettings)
    {
        _databaseSettings = databaseSettings;
    }

    public IEnumerable<Book> GetAllBooks()
    {
        using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand("SELECT * FROM books", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var books = new List<Book>();

                    while (reader.Read())
                    {
                        books.Add(MapToBook(reader));
                    }

                    return books;
                }
            }
        }
    }

    public Book GetBookById(Guid id)
    {
        using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand("SELECT * FROM books WHERE id = @id", connection))
            {
                command.Parameters.AddWithValue("@id", id);
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapToBook(reader);
                    }

                    return null;
                }
            }
        }
    }
    
    public async Task AddBook(Book book, CancellationToken cancellationToken)
    {
        using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
        {
            connection.Open();

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