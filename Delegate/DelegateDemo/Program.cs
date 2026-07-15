// See https://aka.ms/new-console-template for more information


using DelegateDemo.Examples;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("================ CALCULATOR DEMO ================");
        BuildBasicDemo.Run();

        Console.WriteLine("\n================ ORDER EVENT DEMO ================");
        var order =new BuildEventOrder();
        order.Run();
    }
}