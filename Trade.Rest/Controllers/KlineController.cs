using System.Net;
using Microsoft.AspNetCore.Mvc;
using TradeCli;

namespace Trade.Rest.Controllers;

[ApiController]
[Route("[controller]")]
public class KlineController : ControllerBase
{
    public async Task StreamChartData(CancellationToken cancellationToken)
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();

        using var cts = new CancellationTokenSource();
        var clientAndDataLinkedToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken).Token;
        
        string endpoint = "wss://fstream.binance.com/ws/btcusdt@kline_1m";
        var uri = new Uri(endpoint);
        
        await uri.StreamAsync(clientAndDataLinkedToken).SendAsync(ws, clientAndDataLinkedToken);
    }
    
}