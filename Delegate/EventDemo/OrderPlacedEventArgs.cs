namespace EventDemo;

public class OrderPlacedEventArgs: EventArgs
{
    public int OrderId { get; set; }
    public decimal Total { get; set; }
}