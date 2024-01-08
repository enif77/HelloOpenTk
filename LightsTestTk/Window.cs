namespace LightsTestTk;

using System.Runtime.InteropServices;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

using Common;

using LightsTestTk.Extensions;
using LightsTestTk.Models;
using LightsTestTk.Models.Lights;
using LightsTestTk.Models.Materials;
using LightsTestTk.Models.Shaders;


// In this tutorial we focus on how to set up a scene with multiple lights, both of different types but also
// with several point lights
public class Window : GameWindow
{
    private int _viewportSizeScaleFactor = 1;
    
    private Camera _camera;
    private Scene _scene;
    
    private readonly List<Cube> _cubes =
    [
        new Cube(0) {Position = new Vector3(0.0f, 0.0f, 0.0f)},
        new Cube(1) {Position = new Vector3(2.0f, 5.0f, -15.0f)},
        new Cube(2) {Position = new Vector3(-1.5f, -2.2f, -2.5f)},
        new Cube(3) {Position = new Vector3(-3.8f, -2.0f, -12.3f)},
        new Cube(4) {Position = new Vector3(2.4f, -0.4f, -3.5f)},
        new Cube(5) {Position = new Vector3(-1.7f, 3.0f, -7.5f)},
        new Cube(6) {Position = new Vector3(1.3f, -2.0f, -2.5f)},
        new Cube(7) {Position = new Vector3(1.5f, 2.0f, -2.5f)},
        new Cube(8) {Position = new Vector3(1.5f, 0.2f, -1.5f)},
        new Cube(9) {Position = new Vector3(-1.3f, 1.0f, -1.5f)}
    ];
    
    private bool _firstMove = true;
    private Vector2 _lastPos;

    
    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    
    protected override void OnLoad()
    {
        base.OnLoad();
        
        _viewportSizeScaleFactor = Program.Settings.ViewportSizeScaleFactor;
        
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        GL.Enable(EnableCap.DepthTest);
        
        #region Scene
        
        _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
        _scene = new Scene(_camera);
        
        #region Shaders
        
        _scene.AddShader(new NullShader());
        _scene.AddShader(new SkyboxShader());
        _scene.AddShader(new LampShader());
        _scene.AddShader(new CubeShader());
        
        #endregion
        
        #region Skybox
        
        var skybox = new Skybox(
            new SimpleTextureMaterial(
                Texture.LoadFromFile("Resources/Textures/SKYBOX.jpg"),
                _scene.Shaders["skybox"]));
        
        skybox.GenerateVertexObjectBuffer();
        skybox.GenerateVertexArrayObjectForPosTexVbo();
        
        _scene.AddSkybox(skybox);
        
        #endregion
        
        
        #region Cubes
        
        // TODO: Create the first cube and then clone it with setting position to cube clones.
        
        var cubeMaterial = new Material(
            Texture.LoadFromFile("Resources/Textures/container2.png"),
            Texture.LoadFromFile("Resources/Textures/container2_specular.png"),
            _scene.Shaders["cube"]);

        foreach (var cube in _cubes)
        {
            cube.Material = cubeMaterial;

            cube.GenerateVertexObjectBuffer();
            cube.GenerateVertexArrayObjectForPosNormTexVbo();
            
            _scene.AddChild(cube);
        }
        
        #endregion
        
        
        #region Lamps
        
        var lampMaterial = new SimpleColorMaterial(
            new Vector3(1.0f, 1.0f, 1.0f),
            _scene.Shaders["lamp"]);
        
        foreach (var pointLight in _scene.PointLights)
        {
            var lamp = new Cube(-1)
            {
                Material = lampMaterial,
                Position = pointLight.Position
            };
        
            lamp.GenerateVertexObjectBuffer();
            lamp.GenerateVertexArrayObjectForPosNormTexVbo();
            
            _cubes.Add(lamp);
            
            _scene.AddChild(lamp);
        }
        
        #endregion
        
        #endregion
        
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

        UpdateCubes();
        
        _scene.Render();

        SwapBuffers();
    }
    

    private void UpdateCubes()
    {
        var cubeIndex = 0;
        foreach (var cube in _cubes)
        {
            var model = Matrix4.CreateTranslation(cube.Position);
            
            if (cube.Id == -1)
            {
                cube.ModelMatrix = model;
            }
            else
            {
                var angle = 20.0f * cubeIndex;
                model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
                cube.ModelMatrix = model;
           
                cubeIndex++;    
            }
        }
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
            _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
        }
        if (input.IsKeyDown(Keys.S))
        {
            _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
        }
        if (input.IsKeyDown(Keys.A))
        {
            _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
        }
        if (input.IsKeyDown(Keys.D))
        {
            _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
        }
        if (input.IsKeyDown(Keys.Space))
        {
            _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
        }
        if (input.IsKeyDown(Keys.LeftShift))
        {
            _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
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

            _camera.Yaw += deltaX * sensitivity;
            _camera.Pitch -= deltaY * sensitivity;
        }
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        _camera.Fov -= e.OffsetY;
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
        _camera.AspectRatio = Size.X / (float)Size.Y;
    }
}