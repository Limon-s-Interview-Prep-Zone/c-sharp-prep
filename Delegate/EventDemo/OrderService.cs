namespace EventDemo;

public class OrderService
{
    public event EventHandler<OrderPlacedEventArgs>? OrderPlaced;
    
    public void PlaceOrder(int orderId, decimal total)
    {
        Console.WriteLine("Processing order...");

        var args = new OrderPlacedEventArgs
        {
            OrderId = orderId,
            Total = total
        };

        OrderPlaced?.Invoke(this, args);
    }
}