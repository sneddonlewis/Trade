using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Trade.Application;
using Trade.Core.Models;
using Trade.Data.Binance;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(b => b
    .WithOrigins("http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());

app.UseWebSockets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

const string binanceEndpoint = "wss://fstream.binance.com/stream?streams=btcusdt@depth";
app.Map("/order_book", async ctx =>
{
    if (!ctx.WebSockets.IsWebSocketRequest)
    {
        ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }

    var ws = await ctx.WebSockets.AcceptWebSocketAsync();
    
    IOrderBookRepo orderBookRepo = new OrderBookBinanceRepo(binanceEndpoint);

    using var cts = new CancellationTokenSource();
    var clientAndDataLinkedToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, ctx.RequestAborted).Token;
    
    var orderBookEnumerable = orderBookRepo.StreamAsync(clientAndDataLinkedToken);

    await foreach (var orderBook in orderBookEnumerable)
    {
        if (ws.State is WebSocketState.Closed or WebSocketState.Aborted)
        {
            return;
        }
        
        var message = JsonSerializer.Serialize(orderBook);
        var bytes = Encoding.UTF8.GetBytes(message);
        var arrSeg = new ArraySegment<byte>(bytes, 0, bytes.Length);
        await ws.SendAsync(arrSeg, WebSocketMessageType.Text, true, clientAndDataLinkedToken);
        
    }
});

await app.RunAsync();