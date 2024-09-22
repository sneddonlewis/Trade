using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Trade.Application;
using Trade.Core.Models;
using Trade.Data.Binance.Models;

namespace Trade.Data.Binance;

public class OrderBookBinanceRepo(string endpointUri) : IOrderBookRepo
{
    public async IAsyncEnumerable<OrderBook> StreamAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var ws = new ClientWebSocket();
        var buffer = new byte[1024 * 4];
        StringBuilder jsonString = new();

        await ws.ConnectAsync(new Uri(endpointUri), cancellationToken);
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