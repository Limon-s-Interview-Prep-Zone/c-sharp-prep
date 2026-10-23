// See https://aka.ms/new-console-template for more information

using EventDemo;

public class Program
{
    public static void Main()
    {
        var service = new OrderService();

        service.OrderPlaced += (sender, e) =>
        {
            Console.WriteLine($"Order ID: {e.OrderId}");
            Console.WriteLine($"Total: {e.Total}");
        };

        service.PlaceOrder(101, 2500m);
    }
}