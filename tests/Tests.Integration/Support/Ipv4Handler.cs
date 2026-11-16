using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;

namespace Aurore.Archetypes.Api.Tests.Integration.Support;

public static class Ipv4Handler
{
    public static SocketsHttpHandler Create()
    {
        return new SocketsHttpHandler
        {
            ConnectCallback = async (context, cancellationToken) =>
            {
                var host = context.DnsEndPoint.Host;

                // Every *.localhost name is loopback (RFC 6761); asking the resolver for them is slow and sometimes fails on this setup.
                var addresses = host.Equals("localhost", StringComparison.OrdinalIgnoreCase) || host.EndsWith(".localhost", StringComparison.OrdinalIgnoreCase)
                    ? new[] { IPAddress.Loopback }
                    : await Dns.GetHostAddressesAsync(host, AddressFamily.InterNetwork, cancellationToken);
                var socket = new Socket(SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };

                try
                {
                    await socket.ConnectAsync(addresses, context.DnsEndPoint.Port, cancellationToken);

                    return new NetworkStream(socket, ownsSocket: true);
                }
                catch
                {
                    socket.Dispose();

                    throw;
                }
            },
        };
    }
}
