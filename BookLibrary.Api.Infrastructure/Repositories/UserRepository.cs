using System.Data;
using BookLibrary.Api.Application.Interfaces;
using BookLibrary.Api.Domain.Entities;
using BookLibrary.Api.Infrastructure.Settings;
using Npgsql;

namespace BookLibrary.Api.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DatabaseSettings _databaseSettings;

    public UserRepository(DatabaseSettings databaseSettings)
    {
        _databaseSettings = databaseSettings;
    }
    
    public async Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);
            using (var command = new NpgsqlCommand("SELECT * FROM users WHERE username = @username", connection))
            {
                command.Parameters.AddWithValue("@username", username);
                
                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        return MapToUser(reader);
                    }

                    return null;
                }
            }
        }
    }

    public async Task<User?> FindByTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);
            using (var command = new NpgsqlCommand("SELECT * FROM users WHERE refresh_token = @refresh_token", connection))
            {
                command.Parameters.AddWithValue("@refresh_token", refreshToken);
                
                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        return MapToUser(reader);
                    }

                    return null;
                }
            }
        }
    }
    
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
        {
            await connection.OpenAsync(cancellationToken);
            using (var command = new NpgsqlCommand("INSERT INTO users (id, username, password, access_token, refresh_token) VALUES (@id, @username, @password, @access_token, @refresh_token)", connection))
            {
                command.Parameters.AddWithValue("@id", user.Id);
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@password", user.Password);
                command.Parameters.AddWithValue("@access_token", user.AccessToken);
                command.Parameters.AddWithValue("@refresh_token", user.RefreshToken);

                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
            using IDbConnection dbConnection = new NpgsqlConnection(_databaseSettings.ConnectionString);
            using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var command = new NpgsqlCommand("UPDATE users SET username = @username, access_token = @access_token, refresh_token = @refresh_token WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("username", user.Username);
                    command.Parameters.AddWithValue("access_token", user.AccessToken);
                    command.Parameters.AddWithValue("refresh_token", user.RefreshToken);
                    command.Parameters.AddWithValue("id", user.Id);
                    
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
    }
    
    private User MapToUser(IDataReader reader)
    {
        return User.CreateFromDatabase(
            reader.GetGuid(reader.GetOrdinal("id")),
            reader.GetString(reader.GetOrdinal("username")),
            reader.GetString(reader.GetOrdinal("password")),
            reader.GetString(reader.GetOrdinal("access_token")),
            reader.GetString(reader.GetOrdinal("refresh_token"))
        );
    }
}