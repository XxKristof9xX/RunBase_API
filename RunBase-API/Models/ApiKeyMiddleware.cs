using System.Collections.Concurrent;
using System.Collections.Generic;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string API_KEY_NAME = "Authorization";
    private static readonly ConcurrentDictionary<string, (DateTime expiryTime, string role)> _validApiKeys = new();

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var allowedPaths = new[] { "/api/felhasznalok/login", "/api/versenyek" };
        if (allowedPaths.Any(path => context.Request.Path.StartsWithSegments(path)))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(API_KEY_NAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401; 
            await context.Response.WriteAsync("API kulcs szükséges!");
            Console.WriteLine("Nincs API kulcs a headerben.");
            return;
        }

        var apiKey = extractedApiKey.ToString().Replace("Bearer ", "").Trim();
        if (string.IsNullOrEmpty(apiKey))
        {
            context.Response.StatusCode = 403; 
            await context.Response.WriteAsync("Érvénytelen API kulcs!");
            return;
        }
        if (!_validApiKeys.TryGetValue(apiKey, out var apiKeyData) || apiKeyData.expiryTime < DateTime.UtcNow)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Hibás vagy lejárt API kulcs!");
            return;
        }

        await _next(context);
    }

    public static string GenerateApiKey(string role)
    {
        var apiKey = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        if (!_validApiKeys.TryAdd(apiKey, (DateTime.UtcNow.AddHours(1), role)))
        {
            throw new Exception("Nem sikerült az API kulcs mentése!");
        }

        return apiKey; // Az API kulcs visszaadása
    }
    public static void InvalidateApiKey(string apiKey)
    {
        _validApiKeys.TryRemove(apiKey, out _);
    }
}
