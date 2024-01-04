using LightsTestTk.Extensions;

namespace LightsTestTk;

using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

using Common;
using LightsTestTk.Models;
using LightsTestTk.Models.Lights;
using LightsTestTk.Models.Materials;

// In this tutorial we focus on how to set up a scene with multiple lights, both of different types but also
// with several point lights
public class Window : GameWindow
{
    private int _viewportSizeScaleFactor = 1;
    
    private Camera _camera;
    private Scene _scene;
    
    private Cube[] _cubes = new[]
    {
        new Cube(0) { Position = new Vector3(0.0f, 0.0f, 0.0f) },
        new Cube(1) { Position = new Vector3(2.0f, 5.0f, -15.0f) },
        new Cube(2) { Position = new Vector3(-1.5f, -2.2f, -2.5f) },
        new Cube(3) { Position = new Vector3(-3.8f, -2.0f, -12.3f) },
        new Cube(4) { Position = new Vector3(2.4f, -0.4f, -3.5f) },
        new Cube(5) { Position = new Vector3(-1.7f, 3.0f, -7.5f) },
        new Cube(6) { Position = new Vector3(1.3f, -2.0f, -2.5f) },
        new Cube(7) { Position = new Vector3(1.5f, 2.0f, -2.5f) },
        new Cube(8) { Position = new Vector3(1.5f, 0.2f, -1.5f) },
        new Cube(9) { Position = new Vector3(-1.3f, 1.0f, -1.5f) }
    };
    
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
        
        #region Skybox
        
        // Generate skybox VBO and VAO.
        var skybox = new Skybox(
            new SingleTextureMaterial(
                Texture.LoadFromFile("Resources/Textures/SKYBOX.jpg"),
                new Shader(
                    File.ReadAllText("Resources/Shaders/skybox.vert"), 
                    File.ReadAllText("Resources/Shaders/skybox.frag"))));
        
        skybox.VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, skybox.VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, skybox.Vertices.Length * sizeof(float), skybox.Vertices, BufferUsageHint.StaticDraw);
        skybox.VertexArrayObject = GenerateVAOForPosTexVBOs(skybox.Material.Shader);
        
        _scene.AddSkybox(skybox);
        
        #endregion
        
        
        #region Cubes
        
        // TODO: Create the first cube and then clone it with setting position to cube clones.
        
        var cubeMaterial = new Material(
            Texture.LoadFromFile("Resources/Textures/container2.png"),
            Texture.LoadFromFile("Resources/Textures/container2_specular.png"),
            new Shader(
                File.ReadAllText("Resources/Shaders/shader.vert"), 
                File.ReadAllText("Resources/Shaders/lighting.frag")));

        var baseCube = _cubes[0];
        
        var cubeVbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, cubeVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, baseCube.Vertices.Length * sizeof(float), baseCube.Vertices, BufferUsageHint.StaticDraw);
        
        var cubeVao = GenerateVAOForPosNormTexVBOs(cubeMaterial.Shader);
        
        // Generate cube VBOs.
        foreach (var cube in _cubes)
        {
            cube.Material = cubeMaterial;
            cube.VertexBufferObject = cubeVbo;
            cube.VertexArrayObject = cubeVao;
            
            _scene.AddChild(cube);
        }
        
        #endregion
        

        {
            _scene.LampCube.Material = new SingleColorMaterial(
                new Vector3(1.0f, 1.0f, 1.0f),
                new Shader(
                    File.ReadAllText("Resources/Shaders/shader.vert"), 
                    File.ReadAllText("Resources/Shaders/shader.frag")));
            
            _scene.LampCube.VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_scene.LampCube.VertexArrayObject);

            var positionLocation = _scene.LampCube.Material.Shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        }

        #endregion
        
        CursorState = CursorState.Grabbed;
    }

    private int GenerateVAOForPosTexVBOs(IShader shader)
    {
        var vao = GL.GenVertexArray();
        
        GL.BindVertexArray(vao);

        var positionLocation = shader.GetAttribLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            
        var texCoordLocation = shader.GetAttribLocation("aTexCoords");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        return vao;
    }

    
    private int GenerateVAOForPosNormTexVBOs(IShader shader)
    {
        var vao = GL.GenVertexArray();
        
        GL.BindVertexArray(vao);

        var positionLocation = shader.GetAttribLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

        var normalLocation = shader.GetAttribLocation("aNormal");
        GL.EnableVertexAttribArray(normalLocation);
        GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

        var texCoordLocation = shader.GetAttribLocation("aTexCoords");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        
        return vao;
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

        _scene.Skybox?.Render();
        RenderScene();
        RenderLamps();

        SwapBuffers();
    }
    

    private void RenderScene()
    {
        var viewMatrix = _camera.GetViewMatrix();
        var projectionMatrix = _camera.GetProjectionMatrix();
        var cameraPosition = _camera.Position;
        
        // ---
        
        var material = _cubes[0].Material;
        
        material.Use();

        material.Shader.SetMatrix4("view", viewMatrix);
        material.Shader.SetMatrix4("projection", projectionMatrix);
        material.Shader.SetVector3("viewPos", cameraPosition);
        
        /*
           Here we set all the uniforms for the 5/6 types of lights we have. We have to set them manually and index
           the proper PointLight struct in the array to set each uniform variable. This can be done more code-friendly
           by defining light types as classes and set their values in there, or by using a more efficient uniform approach
           by using 'Uniform buffer objects', but that is something we'll discuss in the 'Advanced GLSL' tutorial.
        */
        
        // Directional light
        UpdateDirectionalLightUniforms(material.Shader, _scene.DirectionalLight);

        // Point lights
        foreach (var pointLight in _scene.PointLights)
        {
            UpdatePointLightUniforms(material.Shader, pointLight);
        }

        // Spot light
        _scene.SpotLight.Position = _camera.Position;
        _scene.SpotLight.Direction = _camera.Front;
        UpdateSpotLightUniforms(material.Shader, _scene.SpotLight);
        
        // Draw the cubes
        var cubeIndex = 0;
        foreach (var cube in _cubes)
        {
            GL.BindVertexArray(cube.VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, cube.VertexBufferObject);
                
            Matrix4 model = Matrix4.CreateTranslation(cube.Position);
            float angle = 20.0f * cubeIndex;
            model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
            material.Shader.SetMatrix4("model", model);

            // 36 = 6 sides * 2 triangles * 3 vertices.
            GL.DrawArrays(PrimitiveType.Triangles, 0, cube.IndicesCount);
            
            cubeIndex++;
        }
    }

    
    private void RenderLamps()
    {
        GL.BindVertexArray(_scene.LampCube.VertexArrayObject);

        _scene.LampCube.Material.Use();

        var shader = _scene.LampCube.Material.Shader;
        
        shader.SetMatrix4("view", _camera.GetViewMatrix());
        shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
        
        // We use a loop to draw all the lights at the proper position
        GL.BindBuffer(BufferTarget.ArrayBuffer, _scene.LampCube.VertexBufferObject); // All lamps use the same VBO.
        foreach (var pointLight in _scene.PointLights)
        {
            shader.SetMatrix4(
                "model",
                Matrix4.CreateScale(0.2f) * Matrix4.CreateTranslation(pointLight.Position));

            GL.DrawArrays(PrimitiveType.Triangles, 0, _scene.LampCube.IndicesCount);
        }
    }


    private void UpdateSpotLightUniforms(IShader lightingShader, SpotLight spotLight)
    {
        lightingShader.SetVector3(spotLight.PositionUniformName, spotLight.Position);
        lightingShader.SetVector3(spotLight.DirectionUniformName, spotLight.Direction);
        lightingShader.SetVector3(spotLight.AmbientUniformName, spotLight.Ambient);
        lightingShader.SetVector3(spotLight.DiffuseUniformName, spotLight.Diffuse);
        lightingShader.SetVector3(spotLight.SpecularUniformName, spotLight.Specular);
        lightingShader.SetFloat(spotLight.ConstantUniformName, spotLight.Constant);
        lightingShader.SetFloat(spotLight.LinearUniformName, spotLight.Linear);
        lightingShader.SetFloat(spotLight.QuadraticUniformName, spotLight.Quadratic);
        lightingShader.SetFloat(spotLight.CutOffUniformName, spotLight.CutOff);
        lightingShader.SetFloat(spotLight.OuterCutOffUniformName, spotLight.OuterCutOff);
    }

    private void UpdateDirectionalLightUniforms(IShader lightingShader, DirectionalLight directionalLight)
    {
        lightingShader.SetVector3(directionalLight.DirectionUniformName, directionalLight.Direction);
        lightingShader.SetVector3(directionalLight.AmbientUniformName, directionalLight.Ambient);
        lightingShader.SetVector3(directionalLight.DiffuseUniformName, directionalLight.Diffuse);
        lightingShader.SetVector3(directionalLight.SpecularUniformName, directionalLight.Specular);
    }

    private void UpdatePointLightUniforms(IShader lightingShader, PointLight pointLight)
    {
        lightingShader.SetVector3(pointLight.PositionUniformName, pointLight.Position);
        lightingShader.SetVector3(pointLight.AmbientUniformName, pointLight.Ambient);
        lightingShader.SetVector3(pointLight.DiffuseUniformName, pointLight.Diffuse);
        lightingShader.SetVector3(pointLight.SpecularUniformName, pointLight.Specular);
        lightingShader.SetFloat(pointLight.ConstantUniformName, pointLight.Constant);
        lightingShader.SetFloat(pointLight.LinearUniformName, pointLight.Linear);
        lightingShader.SetFloat(pointLight.QuadraticUniformName, pointLight.Quadratic);
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