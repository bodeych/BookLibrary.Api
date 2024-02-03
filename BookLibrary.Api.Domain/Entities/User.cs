namespace BookLibrary.Api.Domain.Entities;

public sealed class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }
    public string AccessToken { get; private set; }
    public string RefreshToken { get; private set; }
    
    private User(Guid id, string username, string password, string accessToken, string refreshToken)
    {
        Id = id;
        Username = username;
        Password = password;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public static User Create(Guid id, string username, string password, string accessToken, string refreshToken)
    {
        var user = new User(id, username, password, accessToken, refreshToken);
        
        return user;
    }
    public void UpdateAccessToken(string newToken)
    {
        AccessToken = newToken;
    }
    
    public static User CreateFromDatabase(Guid id, string username, string password, string accessToken, string refreshToken)
    {
        return new User(id, username, password, accessToken, refreshToken);
    }
}