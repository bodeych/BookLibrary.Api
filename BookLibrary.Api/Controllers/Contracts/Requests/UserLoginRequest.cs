using System.Runtime.Serialization;

namespace BookLibrary.Api.Controllers.Contracts.Requests;

[DataContract]
public class UserLoginRequest
{
    [DataMember(Name = "username")]
    public string Username { get; init; }
    [DataMember(Name = "password")]
    public string Password { get; init; }
}