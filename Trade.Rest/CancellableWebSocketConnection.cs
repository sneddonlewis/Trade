using System.Net.WebSockets;

namespace Trade.Rest;

public readonly record struct CancellableWebSocketConnection(WebSocket WebSocket, CancellationToken CancellationToken);