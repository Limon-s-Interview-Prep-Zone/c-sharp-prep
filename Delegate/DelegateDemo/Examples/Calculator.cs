namespace DelegateDemo.Examples;

public delegate int MathOperation(int a, int b);

public class Calculator
{
    public int UseFuncCalculation(int a, int b, Func<int, int, int> operation)
    {
        ArgumentNullException.ThrowIfNull(operation);
        return operation(a, b);
    }

    public int DoCalculation(int a, int b, MathOperation operation)
    {
        ArgumentNullException.ThrowIfNull(operation);
        return operation(a, b);
    }

    public int Calculate(int a, int b, string operation)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return operation.ToLowerInvariant() switch
        {
            "add" => a + b,
            "subtract" => a - b,
            "multiply" => a * b,
            "divide" => b != 0 ? a / b : throw new DivideByZeroException("Denominator cannot be zero."),
            _ => throw new ArgumentException($"Invalid operation: '{operation}'", nameof(operation))
        };
    }
}

public static class CalculatorHandler
{
    public static int Add(int a, int b) => a + b;

    public static int Subtract(int a, int b) => a - b;

    public static int Multiply(int a, int b) => a * b;

    public static int Divide(int a, int b) =>
        b != 0 ? a / b : throw new DivideByZeroException("Denominator cannot be zero.");
}

public static class BuildBasicDemo
{
    public static void Run()
    {
        var calculator = new Calculator();

        // 1. Traditional switch-based calculation
        Console.WriteLine($"OOP: {calculator.Calculate(12, 12, "add")}");

        // 2. Custom Delegate execution
        Console.WriteLine($"Delegation: {calculator.DoCalculation(12, 12, CalculatorHandler.Add)}");

        // 3. Multicast delegate behavior
        MathOperation mathOperation = CalculatorHandler.Subtract;
        mathOperation += CalculatorHandler.Subtract;
        mathOperation += CalculatorHandler.Multiply;

        // Note: For static methods, Target is null. Target is populated when referencing instance methods.
        Console.WriteLine($"Delegation Property::: Method: {mathOperation.Method.Name} | Target: {mathOperation.Target ?? "null (static method)"}");

        Console.WriteLine("\n--- Invocation List ---");
        foreach (var item in mathOperation.GetInvocationList())
        {
            Console.WriteLine(item.Method.Name);
        }

        Console.WriteLine("\n----------------Use Func---------");

        // 4. Generic Func<T1, T2, TResult> with Method Group
        Console.WriteLine($"Func: {calculator.UseFuncCalculation(12, 12, CalculatorHandler.Add)}");

        // 5. Generic Func with Anonymous Lambda
        Console.WriteLine($"Func+Anonymous: {calculator.UseFuncCalculation(12, 12, (a, b) => a + b)}");
    }
}