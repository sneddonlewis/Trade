using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace TradeCli;

public class Utils
{
    public static async Task Connect(Uri endpoint, CancellationToken cancellationToken)
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

            var data = JsonSerializer.Deserialize<BinanceKlineDataDto>(fullString);
            
            Console.WriteLine(data.TickerSymbol);
        }
    }
}