﻿using System.Collections.Concurrent;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string API_KEY_NAME = "Authorization";
    private static readonly ConcurrentDictionary<string, (DateTime expiryTime, string role)> _validApiKeys = new();
    private static readonly Dictionary<string, string> _staticApiKeys = new()
    {
        ["ORG-7f3d91a8e8e1b64bb29f2c77ae99c3c8-98fa1b4ec7d243ae88f1c173f8a7bdf1"] = "organizer"
    };

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var allowedPathsWithoutKey = new[]
        {
            "/api/felhasznalok/login",
            "/api/versenyek",
            "/api/forum",
            "/api/versenzo/addVersenyzo"
        };

        if (allowedPathsWithoutKey.Any(path => context.Request.Path.StartsWithSegments(path) && context.Request.Method == "GET"))
        {
            await _next(context);
            return;
        }

        if (context.Request.Path.StartsWithSegments("/api/felhasznalok") && context.Request.Method == "POST")
        {
            await _next(context);
            return;
        }

        if (context.Request.Path.StartsWithSegments("/api/versenyzo/hozzaad") && context.Request.Method == "POST")
        {
            await _next(context);
            return;
        }

        if (context.Request.Path.StartsWithSegments("/api/versenyzo/addVersenyzo") && context.Request.Method == "PUT")
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(API_KEY_NAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API kulcs szükséges!");
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

        var userRole = apiKeyData.role;

        if (userRole == "admin")
        {
            await _next(context);
            return;
        }

        if (context.Request.Path.StartsWithSegments("/api/versenyindulas/jelentkezes") &&
            context.Request.Method == "POST")
        {
            if (userRole != "competitor" && userRole != "organizer" && userRole != "admin")
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Csak competitor, organizer vagy admin jelentkezhet versenyre!");
                return;
            }
        }

        if (userRole == "user" || userRole == "competitor")
        {
            if (
                !(context.Request.Method == "GET" && !context.Request.Path.StartsWithSegments("/api/felhasznalok")) &&
                !context.Request.Path.StartsWithSegments("/api/versenyzo/addVersenyzo") &&
                !(context.Request.Method == "POST" && context.Request.Path.StartsWithSegments("/api/forum")) &&
                !(context.Request.Method == "POST" && context.Request.Path.StartsWithSegments("/api/versenyindulas/jelentkezes") && userRole == "competitor")
            )
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Nincs megfelelő jogosultság!");
                return;
            }
        }


        if (userRole == "organizer" &&
            context.Request.Path.StartsWithSegments("/api/felhasznalok") &&
            context.Request.Method != "GET")
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Nincs megfelelő jogosultság a felhasználók módosításához!");
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
        return apiKey;
    }

    public static void InvalidateApiKey(string apiKey)
    {
        _validApiKeys.TryRemove(apiKey, out _);
    }

    static ApiKeyMiddleware()
    {
        foreach (var kvp in _staticApiKeys)
        {
            _validApiKeys.TryAdd(kvp.Key, (DateTime.MaxValue, kvp.Value));
        }
    }
}
