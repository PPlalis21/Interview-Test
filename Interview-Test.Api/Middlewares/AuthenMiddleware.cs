using System.Security.Cryptography;
using System.Text;

namespace Interview_Test.Middlewares;

public class AuthenMiddleware : IMiddleware
{
    //private const string hashedKey = "";
   private const string hashedKey = "82e3cf278aebb17c955845668c9661005467a8132b2c8fca3dcc0d8010e2af51e23152188989340661c7ead9acc263fa73a4215ae7b6781ce6a794754c6f8a87";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var apiKeyHeader = context.Request.Headers["x-api-key"].ToString();
        if (string.IsNullOrEmpty(apiKeyHeader))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key is missing");
            return;
        }

        var computedHash = ComputeSha512(apiKeyHeader);
        if (!string.Equals(computedHash, hashedKey, StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }

        await next(context);
    }

    private static string ComputeSha512(string input)
    {
        using var sha512 = SHA512.Create();
        var bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}
