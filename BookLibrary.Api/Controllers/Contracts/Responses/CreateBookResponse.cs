using System.Runtime.Serialization;

namespace BookLibrary.Api.Controllers.Contracts.Responses;

[DataContract]
public class CreateBookResponse
{
    [DataMember(Name = "id")]
    public Guid Id { get; init; }
}