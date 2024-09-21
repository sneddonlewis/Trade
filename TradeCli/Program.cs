// See https://aka.ms/new-console-template for more information

using System.Net.WebSockets;
using System.Text;
using static System.Console;

const string binanceEndpoint = "wss://fstream.binance.com/stream?streams=btcusdt@depth";

var ws = new ClientWebSocket();
await ws.ConnectAsync(new Uri(binanceEndpoint), CancellationToken.None);

WriteLine("Connected");

var receiveTask = Task.Run(async () =>
{
    var buffer = new byte[1024];
    while (true)
    {
        var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        WriteLine($"Received: {message}");
    }
});

await receiveTask;
