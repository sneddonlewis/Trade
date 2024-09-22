using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Trade.Application.Binance;

public static class OrderBookBinanceData
{
    public static async Task<OrderBook> ReceiveDataFromBinanceApi(Uri wsEndpoint, CancellationToken cancellationToken)
    { 
        var ws = new ClientWebSocket();
        var buffer = new byte[1024 * 4];
        StringBuilder jsonString = new();

        try
        {
            await ws.ConnectAsync(wsEndpoint, cancellationToken);
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                var messageFragment = Encoding.UTF8.GetString(buffer, 0, result.Count);
                jsonString.Append(messageFragment);
        
                if (!result.EndOfMessage) continue;
                
                var fullString = jsonString.ToString();
                jsonString.Clear();
                    
                var streamData = JsonSerializer.Deserialize<BinanceWsDataStream>(fullString);
                var depthResult = streamData!.Data;

                var orderBook = depthResult.ToOrderBook();

                return orderBook;
            }
        }
        catch (OperationCanceledException) {}
        finally
        {
            if (ws.State is WebSocketState.Open or WebSocketState.CloseSent)
            {
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            }
        }

        return new OrderBook();
    }
}