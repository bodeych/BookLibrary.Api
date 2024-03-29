using System.Runtime.Serialization;

namespace BookLibrary.Api.Controllers.Contracts.Requests;

[DataContract]
public class TokenRefreshRequest
{
    [DataMember(Name = "access_token")]
    public string AccessToken { get; init; }
    [DataMember(Name = "refresh_token")]
    public string RefreshToken { get; init; }
}