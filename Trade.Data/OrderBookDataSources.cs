namespace Trade.Data;

public static class OrderBookDataSources
{
    public static OrderBookDataSource BinanceBtcUsd => Binance("btcusd");
    
    private static OrderBookDataSource Binance(string market) =>
        new(new Uri($"wss://fstream.binance.com/stream?streams={market}t@depth"));
}