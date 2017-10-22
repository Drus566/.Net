using System;

public class Enums
{
    public void MathOp(double x, double y, Operation op)
    {
        double result = 0.0;

        switch (op)
        {
            case Operation.Add:
                result = x + y;
                break;
            case Operation.Subtract:
                result = x - y;
                break;
            case Operation.Multiply:
                result = x * y;
                break;
            case Operation.Divide:
                result = x / y;
                break;
        }
        Console.WriteLine("Результат операции равен {0}", result);
    }
}

enum Days
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}

enum Time : byte
{
    Morning,
    Afternoon,
    Evening,
    Night
}

public enum Operation
{
    Add = 1,
    Subtract,
    Multiply,
    Divide
}