namespace BookLibrary.Api.Domain.Entities;

public sealed class Review
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Comment { get; set; }
    public double Rating { get; set; }
}