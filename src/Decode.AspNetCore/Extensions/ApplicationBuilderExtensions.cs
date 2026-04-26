using Decode.AspNetCore.Middlewares;

namespace Microsoft.AspNetCore.Builder;

public static class DecodeApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the Decode global exception handler to the application pipeline.
    /// </summary>
    public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}