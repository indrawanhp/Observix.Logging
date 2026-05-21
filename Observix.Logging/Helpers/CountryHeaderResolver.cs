using Microsoft.AspNetCore.Http;

namespace Observix.Logging.Helpers;

public static class CountryHeaderResolver
{
    public static string Resolve(HttpRequest request)
    {
        var country =
            request.Headers["x-country"]
                .FirstOrDefault()

            ?? request.Headers["x-obras-country"]
                .FirstOrDefault()

            ?? request.Headers["x-dnet-country"]
                .FirstOrDefault()

            ?? "ALL";

        return country.ToUpper();
    }
}