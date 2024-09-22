using System.Net;
using System.Net.WebSockets;
using System.Text;

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

app.Map("/order_book", async ctx =>
{
    if (!ctx.WebSockets.IsWebSocketRequest)
    {
        ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }

    int counter = 1;
    var ws = await ctx.WebSockets.AcceptWebSocketAsync();

    while (true)
    {
        if (ws.State is WebSocketState.Closed or WebSocketState.Aborted)
        {
            return;
        }
        
        string message = $"Counter {counter++}";
        var bytes = Encoding.UTF8.GetBytes(message);
        var arrSeg = new ArraySegment<byte>(bytes, 0, bytes.Length);
        await ws.SendAsync(arrSeg, WebSocketMessageType.Text, true, CancellationToken.None);

        await Task.Delay(1000);
    }

});

await app.RunAsync();