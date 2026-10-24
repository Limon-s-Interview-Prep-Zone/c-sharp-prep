// See https://aka.ms/new-console-template for more information

// TaskBasedAsync.TaskBasedAsync.BasicTask();
//
// await TaskBasedAsync.TaskBasedAsync.TaskWhenAll();
//
// TaskBasedAsync.TaskBasedAsync.ContinuationTask();
//
// TaskBasedAsync.TaskBasedAsync.ParallelTask();

Console.Write("1 ");
var work = DoWorkAsync();
Console.Write("2 ");
// await work;
Console.Write("3 ");
async Task DoWorkAsync()
{
    Console.Write("4 ");
    Thread.Sleep(10000);
    Console.Write("5 ");
    await Task.FromResult(100);
    await Task.Delay(1);
    Console.Write("6 ");
}