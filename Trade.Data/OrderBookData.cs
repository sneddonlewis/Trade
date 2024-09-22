using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Trade.Core.Models;
using Trade.Data.Binance;
using Trade.Data.Binance.Models;

namespace Trade.Data;

public static class OrderBookData
{
    public static async IAsyncEnumerable<OrderBook> StreamAsync(
        this OrderBookDataSource source, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var ws = new ClientWebSocket();
        var buffer = new byte[1024 * 4];
        StringBuilder jsonString = new();

        await ws.ConnectAsync(source.Endpoint, cancellationToken);
        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
            var messageFragment = Encoding.UTF8.GetString(buffer, 0, result.Count);
            jsonString.Append(messageFragment);
        
            if (!result.EndOfMessage) continue;
                
            var fullString = jsonString.ToString();
            jsonString.Clear();
                    
            var streamData = JsonSerializer.Deserialize<BinanceWsDataStreamDto>(fullString);
            var depthResult = streamData!.Data;

            var orderBook = depthResult.ToOrderBook();

            yield return orderBook;
        }
    }
}