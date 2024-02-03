namespace BookLibrary.Api.Domain.Entities;

public sealed class Review
{
    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public Guid UserId { get; private set; }
    public string Comment { get; private set; }
    public double Rating { get; private set; }
    
    private Review(Guid id, Guid bookId, Guid userId, string comment, double rating)
    {
        Id = id;
        BookId = bookId;
        UserId = userId;
        Comment = comment;
        Rating = rating;
    }
    
    private static bool IsValidRating(double rating) => rating >= 1 && rating <= 10;
    
    public static Review Create(Guid id, Guid bookId, Guid userId, string comment, double rating)
    {
        if (!IsValidRating(rating))
        {
            throw new ArgumentException(nameof(rating));
        }
        
        var review = new Review(id, bookId, userId, comment, rating);
        
        return review;
    }
    
    public static Review CreateFromDatabase(Guid id, Guid bookId, Guid userId, string comment, double rating)
    {
        return new Review(id, bookId, userId, comment, rating);
    }
}