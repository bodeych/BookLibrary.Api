namespace BookLibrary.Api.ServiceCollectionExtensions;

public static class HttpExtensions
{
    public static Guid GetIdUser(this HttpContext httpContext)
    {
        if (httpContext.User is null)
            return Guid.Empty;

        var claim = httpContext.User.Claims.Single(x => x.Type == "userId");

        return Guid.Parse(claim.Value);
    }
}