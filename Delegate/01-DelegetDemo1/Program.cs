using System;

namespace DelegetDemo1
{
    public class Student
    {
        public void Add(int a, int b)
        {
            Console.WriteLine($"The sum of a {a} and b {b} is ={(a + b)}");
        }
        public static void Subtract(int a, int b)
        {
            Console.WriteLine($"The Subtraction of a {a} and b {b} is ={(a - b)}");
        }
    }

    public delegate void AddDelegate(int a, int b);
    public delegate void SubtractDelegate(int a, int b);
    public class DelegateDemo
    {
        public void Add(int a, int b)
        {
            Console.WriteLine($"The sum of a {a} and b {b} is ={(a + b)}");
        }
        public static void Subtract(int a, int b)
        {
            Console.WriteLine($"The Subtraction of a {a} and b {b} is ={(a - b)}");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Student student = new Student();
            // student.Add(10, 12);
            // Student.Subtract(20, 10);
            DelegateDemo delegateDemo = new DelegateDemo();
            AddDelegate d = new AddDelegate(delegateDemo.Add);
            d(10, 20);
            SubtractDelegate sd = new SubtractDelegate(DelegateDemo.Subtract);
            sd.Invoke(20, 10);
        }
    }
}