﻿namespace LarsProjekt.Authentication;

internal static class ApiKeyAuthenticationScheme
{
    public const string DefaultScheme = "ApiKey";
    public const string ApiKeySectionName = "Authentication:ApiKey";
    public const string ApiKeyHeaderName = "X-Api-Key";
}
