﻿using LightsTestTk.Extensions;
using LightsTestTk.Models.Lights;

namespace LightsTestTk;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

using LightsTestTk.Models;

public static class Program
{
    /// <summary>
    /// Global application settings.
    /// </summary>
    public static Settings Settings { get; private set; } = Settings.DefaultSettings;
    
    
    private static void Main()
    {
        Configure();
        
        // var light = new PointLight(0);
        //
        // light.SetLightAttenuationConstants(5.0f);
        // light.SetLightAttenuationConstants(3250.0f);
        // light.SetLightAttenuationConstants(1000.0f);
        // light.SetLightAttenuationConstants(500.0f);
        // light.SetLightAttenuationConstants(90.0f);
        // light.SetLightAttenuationConstants(100.0f);
        //
        // return;
        
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

        using (var window = new Window(
                   GameWindowSettings.Default,
                   nativeWindowSettings,
                   new Game()))
        {
            window.Run();
        }
    }

    
    private static void Configure()
    {
        const string configFileName = Defaults.ConfigFileName;
        
        // We are reading and writing config file in user's home directory.
        var configFileRootPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        
        // If config file does not exist, create it with default values.
        var configPath = Path.Combine(
            configFileRootPath,
            configFileName);
        if (File.Exists(configPath) == false)
        {
            try
            {
                File.WriteAllText(configPath, Settings.DefaultSettings.ToJson());
            }
            catch (IOException)
            {
                // We are OK with an IO related exception.
            }
        }
        
        var builder = new ConfigurationBuilder()
            .AddJsonFile(
                new PhysicalFileProvider(      // This provider allows us to load config from an absolute path.
                    configFileRootPath,
                    ExclusionFilters.System),  // We are reading from an hidden file.
                configFileName,
                optional: true,
                reloadOnChange: false);  // This slows down app startup. Also - not sure, how to react to config reloads.

        var config = builder.Build();

        Settings = config.GetSection("Settings")
            .Get<Settings>() ?? Settings.DefaultSettings;
    }
}