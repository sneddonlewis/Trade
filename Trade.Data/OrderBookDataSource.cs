namespace Trade.Data;

public readonly record struct OrderBookDataSource
{
    internal Uri Endpoint { get; init; }

    internal OrderBookDataSource(Uri endpoint)
    {
        Endpoint = endpoint;
    }
}