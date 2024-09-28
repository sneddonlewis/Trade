using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace TradeCli;

public static class KlineStream
{
    public static async IAsyncEnumerable<BinanceKlineDto> StreamAsync(
        this Uri endpoint, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var ws = new ClientWebSocket();
        var buffer = new byte[1024 * 4];
        StringBuilder jsonString = new();

        await ws.ConnectAsync(endpoint, cancellationToken);
        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
            var messageFragment = Encoding.UTF8.GetString(buffer, 0, result.Count);
            jsonString.Append(messageFragment);
        
            if (!result.EndOfMessage) continue;
                
            var fullString = jsonString.ToString();
            jsonString.Clear();
                    
            var streamData = JsonSerializer.Deserialize<BinanceKlineDataDto>(fullString);

            yield return streamData!.Kline;
        }
    }
    
}