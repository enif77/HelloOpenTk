namespace LightsTestTk;

using System.Runtime.InteropServices;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

// In this tutorial we focus on how to set up a scene with multiple lights, both of different types but also
// with several point lights
public class Window : GameWindow
{
    private int _viewportSizeScaleFactor = 1;
    
    private readonly Game _game;
    
    private bool _firstMove = true;
    private Vector2 _lastPos;

    
    public Window(
        GameWindowSettings gameWindowSettings,
        NativeWindowSettings nativeWindowSettings,
        Game game)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        _game = game ?? throw new ArgumentNullException(nameof(game));
    }

    
    protected override void OnLoad()
    {
        base.OnLoad();
        
        _game.Initialize(Size.X, Size.Y);
        
        _viewportSizeScaleFactor = Program.Settings.ViewportSizeScaleFactor;
        
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        GL.Enable(EnableCap.DepthTest);
        
        CursorState = CursorState.Grabbed;
    }
    

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        // MacOS needs to update the viewport on each frame.
        // FramebufferSize returns correct values, only Cocoa visual render size is halved.
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && _viewportSizeScaleFactor > 1)
        {
            var cs = ClientSize;
            GL.Viewport(0, 0, cs.X * _viewportSizeScaleFactor, cs.Y * _viewportSizeScaleFactor);
        }
        
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        
        _game.Render();

        SwapBuffers();
    }
    
    
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (!IsFocused)
        {
            return;
        }

        var input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        const float cameraSpeed = 1.5f;
        const float sensitivity = 0.2f;

        if (input.IsKeyDown(Keys.W))
        {
            _game.Camera.Position += _game.Camera.Front * cameraSpeed * (float)e.Time; // Forward
        }
        if (input.IsKeyDown(Keys.S))
        {
            _game.Camera.Position -= _game.Camera.Front * cameraSpeed * (float)e.Time; // Backwards
        }
        if (input.IsKeyDown(Keys.A))
        {
            _game.Camera.Position -= _game.Camera.Right * cameraSpeed * (float)e.Time; // Left
        }
        if (input.IsKeyDown(Keys.D))
        {
            _game.Camera.Position += _game.Camera.Right * cameraSpeed * (float)e.Time; // Right
        }
        if (input.IsKeyDown(Keys.Space))
        {
            _game.Camera.Position += _game.Camera.Up * cameraSpeed * (float)e.Time; // Up
        }
        if (input.IsKeyDown(Keys.LeftShift))
        {
            _game.Camera.Position -= _game.Camera.Up * cameraSpeed * (float)e.Time; // Down
        }

        var mouse = MouseState;

        if (_firstMove)
        {
            _lastPos = new Vector2(mouse.X, mouse.Y);
            _firstMove = false;
        }
        else
        {
            var deltaX = mouse.X - _lastPos.X;
            var deltaY = mouse.Y - _lastPos.Y;
            _lastPos = new Vector2(mouse.X, mouse.Y);

            _game.Camera.Yaw += deltaX * sensitivity;
            _game.Camera.Pitch -= deltaY * sensitivity;
        }
        
        _game.Update((float)e.Time);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        _game.Camera.Fov -= e.OffsetY;
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
        _game.Camera.AspectRatio = Size.X / (float)Size.Y;
    }
}