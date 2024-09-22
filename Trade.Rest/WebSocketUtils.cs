using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Trade.Rest;

public static class WebSocketUtils
{
    public static async Task SendAsync<T>(this IAsyncEnumerable<T> sequence, WebSocket ws, CancellationToken cancellationToken)
    {
        await foreach (var data in sequence.WithCancellation(cancellationToken))
        {
            if (ws.State is WebSocketState.Closed or WebSocketState.Aborted)
            {
                return;
            }
        
            var message = JsonSerializer.Serialize(data, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
            var bytes = Encoding.UTF8.GetBytes(message);
            var arrSeg = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await ws.SendAsync(arrSeg, WebSocketMessageType.Text, true, cancellationToken);
        }
    }

    public static async Task<CancellableWebSocketConnection> AcceptWebSocketConnectionAsync(HttpContext context)
    {
        // TODO - rethink this a bit
        var ws = await context.WebSockets.AcceptWebSocketAsync();

        using var cts = new CancellationTokenSource();
        var clientAndDataLinkedToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, context.RequestAborted).Token;
        return new CancellableWebSocketConnection(ws, clientAndDataLinkedToken);
    }
}