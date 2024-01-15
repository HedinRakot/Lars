namespace LarsProjekt.Authentication;

internal static class ApiKeyAuthenticationScheme
{
    public const string DefaultScheme = "ApiKey";
    public const string ApiKeySectionName = "Authentication";
    public const string ApiKeyHeaderName = "X-Api-Key";
}
