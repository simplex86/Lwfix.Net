using System;
using SimplexLab.Fixed.Physics.JDemo.Renderer;
using Silk.NET.Windowing;

namespace SimplexLab.Fixed.Physics.JDemo;

public static class Program
{
    private static void PrintException(Exception ex, string info)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.GetType().Name);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(info);
        Console.ResetColor();
        Console.WriteLine(ex.Message);
    }

    public static void Main()
    {
        Logger.Listener = (level, message) =>
        {
            ConsoleColor color = level switch
            {
                Logger.LogLevel.Information => ConsoleColor.Green,
                Logger.LogLevel.Warning     => ConsoleColor.Yellow,
                Logger.LogLevel.Error       => ConsoleColor.Red,
                _                           => ConsoleColor.Gray,
            };

            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"[Jitter] {level}: {message}");
            Console.ForegroundColor = previous;
        };

        CreationSettings settings = new(1200, 800, "Lwfix.Net-Physics - Jitter2 Demo");

        try
        {
            IWindow window = (IWindow)Window.Create(settings);

            var playground = new Playground(window);
            playground.RegisterDemo(new Demo02());
            playground.RegisterDemo(new Demo03());
            playground.RegisterDemo(new Demo04());
            playground.RegisterDemo(new Demo09());
            playground.RegisterDemo(new Demo10());
            playground.RegisterDemo(new Demo11());
            playground.RegisterDemo(new Demo13());
            playground.RegisterDemo(new Demo22());
            playground.RegisterDemo(new Demo23());
            playground.RegisterDemo(new Demo28());

            playground.Open(settings);
        }
        catch (DllNotFoundException ex)
        {
            PrintException(ex, "Unable to load native library.");
        }
        catch (Exception ex)
        {
            PrintException(ex, "Unhandled exception.");
        }
    }
}
