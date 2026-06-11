using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RecAI.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var sub = principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
                  ?? throw new UnauthorizedAccessException("User id claim is missing.");
        return Guid.Parse(sub);
    }
}