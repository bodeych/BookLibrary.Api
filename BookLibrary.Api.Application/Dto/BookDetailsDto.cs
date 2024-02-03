namespace BookLibrary.Api.Application.Dto;

public class BookDetailsDto
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string Title { get; init; }
    public required string Author { get; init; }
    public required string Genre { get; init; }
    public required int PublicationYear { get; init; }
}