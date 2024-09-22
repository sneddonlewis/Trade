using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Trade.Data;

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

        var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();

        using var cts = new CancellationTokenSource();
        var clientAndDataLinkedToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken).Token;

        await OrderBookDataSources.BinanceBtcUsd.StreamAsync(clientAndDataLinkedToken).SendAsync(ws, clientAndDataLinkedToken);
    }
}