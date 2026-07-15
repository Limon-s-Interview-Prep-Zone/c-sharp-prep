/*
              CreateOrder()
                 │
                 ▼
         "Order created"
                 │
                 ▼
       OrderCreated.Invoke()
                 │
         ┌───────┴────────┐
         ▼                ▼
   SendEmail()      SendNotification()
  
 */

namespace DelegateDemo.Examples;



public class OrderEventArgs : EventArgs
{
    public int OrderId { get; set; }
    public string Username { get; set; }
}
public class OrderService
{
    public event EventHandler<OrderEventArgs>? OrderCreated;

    public void Create(OrderEventArgs @event)
    {
        Console.WriteLine($"Order created for Id:{@event.OrderId}");

        OrderCreated?.Invoke(this, @event);
    }
}

public class EmailService
{
    public void SendEmail(object? sender, OrderEventArgs e)
    {
        Console.WriteLine($"Email sent for order Id:{e.OrderId}");
    }
}

public class NotificationService
{
    public void SendNotification(object? sender, OrderEventArgs e)
    {
        Console.WriteLine($"Notification sent for Id: {e.OrderId}");
    }
}


public class BuildEventOrder
{
    public void Run()
    {
        var orderService = new OrderService();

        var emailService = new EmailService();
        var notificationService = new NotificationService();

        orderService.OrderCreated += emailService.SendEmail;
        orderService.OrderCreated += notificationService.SendNotification;

        orderService.Create(new OrderEventArgs(){OrderId= 12345, Username = "Limon"});
    }
}