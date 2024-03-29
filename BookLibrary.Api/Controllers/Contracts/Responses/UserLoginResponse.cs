using System.Runtime.Serialization;

namespace BookLibrary.Api.Controllers.Contracts.Responses;

[DataContract]
public class UserLoginResponse
{
    [DataMember(Name = "access_token")]
    public required string AccessToken { get; init; }
    [DataMember(Name = "refresh_token")]
    public required string RefreshToken { get; init; }
}