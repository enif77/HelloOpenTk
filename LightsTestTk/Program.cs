using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

using LightsTestTk.Models;


namespace LightsTestTk;

public static class Program
{
    /// <summary>
    /// Global application settings.
    /// </summary>
    public static Settings Settings { get; private set; } = Settings.DefaultSettings;
    
    
    private static void Main()
    {
        var nativeWindowSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(Settings.WindowWidth, Settings.WindowHeight),
            //ClientSize = new Vector2i(1280, 800),
            //ClientSize = new Vector2i(3440, 1440),
            
            Title = Defaults.AppVersionInfo,
            
            // This is needed to run on macos
            Flags = ContextFlags.ForwardCompatible,
            Vsync = Settings.EnableVSync ? VSyncMode.Adaptive : VSyncMode.Off,
            WindowState = Settings.EnableFullscreen ? WindowState.Fullscreen : WindowState.Normal
        };

        using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
        {
            window.Run();
        }
    }
}