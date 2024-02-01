using System.Runtime.Serialization;

namespace BookLibrary.Api.Controllers.Contracts.Requests;

[DataContract]
public sealed class CreateBookRequest
{
    [DataMember(Name = "user_id")]
    public Guid UserId { get; init; }
    [DataMember(Name = "title")]
    public string Title { get; set; }
    [DataMember(Name = "author")]
    public string Author { get; set; }
    [DataMember(Name = "genre")]
    public string Genre { get; set; }
    [DataMember(Name = "publication_year")]
    public int PublicationYear { get; set; }
}