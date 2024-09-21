// See https://aka.ms/new-console-template for more information

using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using TradeCli;
using static System.Console;

const string binanceEndpoint = "wss://fstream.binance.com/stream?streams=btcusdt@depth";

var ws = new ClientWebSocket();
await ws.ConnectAsync(new Uri(binanceEndpoint), CancellationToken.None);

WriteLine("Connected");

var receiveTask = Task.Run(async () =>
{
    var buffer = new byte[1024 * 4];
    StringBuilder jsonString = new();
    while (true)
    {
        var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        
        var messageFragment = Encoding.UTF8.GetString(buffer, 0, result.Count);
        jsonString.Append(messageFragment);

        if (result.EndOfMessage)
        {
            var fullString = jsonString.ToString();
            jsonString.Clear();
            
            var streamData = JsonSerializer.Deserialize<BinanceWsDataStream>(fullString);
            var depthResult = streamData!.Data;
        
            WriteLine("Received");
            WriteLine();
            WriteLine($"Event: {depthResult.Event}");
            WriteLine($"Timestamp: {depthResult.EventTimeStamp}");
            WriteLine($"Symbol: {depthResult.Symbol}");
            WriteLine($"Asks: {depthResult.Asks}");
            WriteLine($"Bids: {depthResult.Bids}");
            WriteLine();
        } 
    }
});

await receiveTask;
