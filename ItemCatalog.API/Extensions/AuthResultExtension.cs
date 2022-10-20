using ItemCatalog.API.Models;

namespace ItemCatalog.API.Extensions;

public static class AuthResultExtension
{
    public static AuthResult AddError(this AuthResult authResult, string errorMessage)
    {
        authResult.Errors.Add(errorMessage);
        return authResult;
    }

    public static AuthResult AddToken(this AuthResult authResult, string token)
    {
        authResult.Token = token;
        authResult.Result = true;
        return authResult;
    }
}