namespace BookLibrary.Api.Domain.Entities;

public sealed class Book
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string Genre { get; private set; }
    public int PublicationYear { get; private set; }

    private Book(Guid id, Guid userId, string title, string author, string genre, int publicationYear)
    {
        Id = id;
        UserId = userId;
        Title = title;
        Author = author;
        Genre = genre;
        PublicationYear = publicationYear;
    }
    
    private static bool IsValidYear(int publicationYear) => publicationYear >= 1800 && publicationYear <= 2025;

    public static Book Create(Guid userId, string title, string author, string genre, int publicationYear)
    {
        if (!IsValidYear(publicationYear))
        {
            throw new ArgumentException(nameof(publicationYear));
        }

        var book = new Book(Guid.NewGuid(), userId, title, author, genre, publicationYear);

        return book;
    }
    
    public static Book CreateFromDatabase(Guid id, Guid userId, string title, string author, string genre, int publicationYear)
    {
        return new Book(id, userId, title, author, genre, publicationYear);
    }
}