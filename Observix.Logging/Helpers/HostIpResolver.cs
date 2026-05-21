using System.Net;

namespace Observix.Logging.Helpers;

public static class HostIpResolver
{
    public static string? Resolve()
    {
        return Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(x =>
                x.AddressFamily ==
                System.Net.Sockets.AddressFamily.InterNetwork)
            ?.ToString();
    }
}