namespace TaskBasedAsync;

public class TestTask
{
    public async Task TestAsync()
    {
        Console.Write("1 ");
        var work = DoWorkAsync();
        Console.Write("2 ");
        await work;
        Console.Write("3 ");
    }
    
    async Task DoWorkAsync()
    {
        Console.Write("4 ");
        Thread.Sleep(1000);
        Console.Write("5 ");
        await Task.Delay(100);
        Console.Write("6 ");
    }
}