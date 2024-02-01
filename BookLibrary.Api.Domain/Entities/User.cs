namespace BookLibrary.Api.Domain.Entities;

public sealed class User
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}