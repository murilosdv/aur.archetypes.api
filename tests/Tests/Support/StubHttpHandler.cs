using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aurore.Archetypes.Api.Tests.Support;

/// <summary>
/// Answers every request with the given status and body, and keeps the request it received, so a client is tested without a network.
/// </summary>
internal sealed class StubHttpHandler(HttpStatusCode status, string body = "") : HttpMessageHandler
{
    public HttpRequestMessage? Request { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Request = request;

        return Task.FromResult(new HttpResponseMessage(status)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
        });
    }
}
