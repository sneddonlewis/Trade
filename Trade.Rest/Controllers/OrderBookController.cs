using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Trade.Application;
using Trade.Data.Binance;

namespace Trade.Rest.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderBookController : ControllerBase
{
    public async Task StreamOrderBook(CancellationToken cancellationToken)
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }
        const string binanceEndpoint = "wss://fstream.binance.com/stream?streams=btcusdt@depth";

        var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();
    
        IOrderBookRepo orderBookRepo = new OrderBookBinanceRepo(binanceEndpoint);

        using var cts = new CancellationTokenSource();
        var clientAndDataLinkedToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken).Token;
    
        var orderBookEnumerable = orderBookRepo.StreamAsync(clientAndDataLinkedToken);

        await foreach (var orderBook in orderBookEnumerable)
        {
            if (ws.State is WebSocketState.Closed or WebSocketState.Aborted)
            {
                return;
            }
        
            var message = JsonSerializer.Serialize(orderBook, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
            var bytes = Encoding.UTF8.GetBytes(message);
            var arrSeg = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await ws.SendAsync(arrSeg, WebSocketMessageType.Text, true, clientAndDataLinkedToken);
        
        }
    }
    
}