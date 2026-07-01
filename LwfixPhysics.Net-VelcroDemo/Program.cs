using Silk.NET.Windowing;
using SimplexLab.LwfixPhysics.VelcroDemo.Demos;

namespace SimplexLab.LwfixPhysics.VelcroDemo;

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
        var options = WindowOptions.Default with
        {
            Size = new Silk.NET.Maths.Vector2D<int>(1280, 800),
            Title = "LwfixPhysics.Net - Velcro Demo",
            API = GraphicsAPI.Default with { API = ContextAPI.OpenGL, Version = new(3, 3) },
            WindowState = WindowState.Normal,
            VSync = false,
            ShouldSwapAutomatically = true
        };

        try
        {
            IWindow window = Window.Create(options);
            var playground = new Playground(window);

            playground.RegisterDemo(new D01_SingleFixture());
            playground.RegisterDemo(new D04_StackedBodies());
            playground.RegisterDemo(new D07_Friction());
            playground.RegisterDemo(new D08_DistanceAngleJoint());
            playground.RegisterDemo(new D09_DynamicJoints());
            playground.RegisterDemo(new D14_RacingCar());
            playground.RegisterDemo(new D16_BreakableBody());

            playground.Open();
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
