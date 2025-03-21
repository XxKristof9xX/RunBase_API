using System.Collections.Concurrent;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string API_KEY_NAME = "Authorization";
    private static readonly ConcurrentDictionary<string, DateTime> _validApiKeys = new();

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
        if (!_validApiKeys.TryGetValue(apiKey, out var expiryTime) || expiryTime < DateTime.UtcNow)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Hibás vagy lejárt API kulcs!");
            return;
        }

        await _next(context);
    }

    public static string GenerateApiKey()
    {
        var apiKey = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        if (!_validApiKeys.TryAdd(apiKey, DateTime.UtcNow.AddHours(1)))
        {
            throw new Exception("Nem sikerült az API kulcs mentése!");
        }
        return apiKey;
    }
    public static void InvalidateApiKey(string apiKey)
    {
        _validApiKeys.TryRemove(apiKey, out _);
    }
}
