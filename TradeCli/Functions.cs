namespace TradeCli;

public static class Functions
{
    public static void ForEach<T>(this IEnumerable<T> iterable, Action<T> action)
    {
        foreach (T item in iterable)
        {
            action(item);
        }
    }
}