using System.Runtime.Serialization;

namespace BookLibrary.Api.Controllers.Contracts.Responses;

[DataContract]
public sealed class BookDetailsResponse
{
    [DataMember(Name = "id")]
    public required Guid Id { get; init; }
    [DataMember(Name = "user_id")]
    public required Guid UserId { get; init; }
    [DataMember(Name = "title")]
    public required string Title { get; init; }
    [DataMember(Name = "author")]
    public required string Author { get; init; }
    [DataMember(Name = "genre")]
    public required string Genre { get; init; }
    [DataMember(Name = "publication_year")]
    public required int PublicationYear { get; init; }
}