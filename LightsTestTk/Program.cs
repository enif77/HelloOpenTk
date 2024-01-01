using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace LightsTestTk;

public static class Program
{
    private static void Main()
    {
        var nativeWindowSettings = new NativeWindowSettings()
        {
            //ClientSize = new Vector2i(800, 600),
            ClientSize = new Vector2i(1280, 800),
            
            Title = "LearnOpenTK - Multiple lights",
            // This is needed to run on macos
            Flags = ContextFlags.ForwardCompatible,
        };

        using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
        {
            window.Run();
        }
    }
}