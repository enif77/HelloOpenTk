using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

using Common;
using LightsTestTk.Models;


namespace LightsTestTk;

// In this tutorial we focus on how to set up a scene with multiple lights, both of different types but also
// with several point lights
public class Window : GameWindow
{
    private readonly Cube[] _cubes = new[]
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
    
    private readonly Cube _lampCube = new Cube(-1);  // Cube.Id = -1 means it's a lamp.
    
    private readonly DirectionalLight _directionalLight = new DirectionalLight();
    private readonly SpotLight _spotLight = new SpotLight();
    
    // We need the point lights' positions to draw the lamps and to get light the materials properly
    private readonly PointLight[] _pointLights =
    [
        new PointLight(0)
        {
            Position = new Vector3(0.7f, 0.2f, 2.0f),
        },
        new PointLight(1)
        {
            Position = new Vector3(2.3f, -3.3f, -4.0f),
        },
        new PointLight(2)
        {
            Position = new Vector3(-4.0f, 2.0f, -12.0f),
        },
        new PointLight(3)
        {
            Position = new Vector3(0.0f, 0.0f, -3.0f),
        }
    ];

    // Cubes.
    private Shader _lightingShader;
    private Texture _diffuseMap;
    private Texture _specularMap;
    
    // Lamps.
    private int _vaoLamp;
    private Shader _lampShader;
    
    // Skybox.
    private Shader _skyboxShader;
    private Texture _skyboxTexture;
    private readonly Skybox _skybox = new Skybox(0);
    
    private Camera _camera;
    private bool _firstMove = true;
    private Vector2 _lastPos;

    
    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    
    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        
        GL.Enable(EnableCap.DepthTest);

        // Load shaders.
        _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
        _lampShader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        _skyboxShader = new Shader("Shaders/skybox.vert", "Shaders/skybox.frag");
        
        // Generate skybox VBO and VAO.
        _skybox.VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _skybox.VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _skybox.Vertices.Length * sizeof(float), _skybox.Vertices, BufferUsageHint.StaticDraw);
        _skybox.VertexArrayObject = GenerateVAOForPosTexVBOs();
        
        
        // Generate cube VBOs.
        foreach (var cube in _cubes)
        {
            cube.VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, cube.VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, cube.Vertices.Length * sizeof(float), cube.Vertices, BufferUsageHint.StaticDraw);    
        }
        
        // Generate VAO for cubes.
        var cubeVao = GenerateVAOForPosNormTexVBOs();
        foreach (var cube in _cubes)
        {
            cube.VertexArrayObject = cubeVao;
        }
        

        {
            _vaoLamp = GL.GenVertexArray();
            GL.BindVertexArray(_vaoLamp);

            var positionLocation = _lampShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        }

        _diffuseMap = Texture.LoadFromFile("Resources/container2.png");
        _specularMap = Texture.LoadFromFile("Resources/container2_specular.png");
        _skyboxTexture = Texture.LoadFromFile("Resources/SKYBOX.jpg");

        _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

        CursorState = CursorState.Grabbed;
    }

    private int GenerateVAOForPosTexVBOs()
    {
        var vao = GL.GenVertexArray();
        
        GL.BindVertexArray(vao);

        var positionLocation = _skyboxShader.GetAttribLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            
        var texCoordLocation = _skyboxShader.GetAttribLocation("aTexCoords");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        return vao;
    }

    
    private int GenerateVAOForPosNormTexVBOs()
    {
        var vao = GL.GenVertexArray();
        
        GL.BindVertexArray(vao);

        var positionLocation = _lightingShader.GetAttribLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

        var normalLocation = _lightingShader.GetAttribLocation("aNormal");
        GL.EnableVertexAttribArray(normalLocation);
        GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

        var texCoordLocation = _lightingShader.GetAttribLocation("aTexCoords");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        
        return vao;
    }
    

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        // MacOS needs to update the viewport on each frame.
        // FramebufferSize returns correct values, only Cocoa visual render size is halved.
        // if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        // {
        //     var cs = ClientSize;
        //     GL.Viewport(0, 0, cs.X * 2, cs.Y * 2);
        // }
        
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        
        RenderSkybox();
        
        RenderCubes();
        RenderLamps();

        SwapBuffers();
    }

    private void RenderCubes()
    {
        _diffuseMap.Use(TextureUnit.Texture0);
        _specularMap.Use(TextureUnit.Texture1);
        
        _lightingShader.Use();

        _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
        _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

        _lightingShader.SetVector3("viewPos", _camera.Position);

        _lightingShader.SetInt("material.diffuse", 0);
        _lightingShader.SetInt("material.specular", 1);
        _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
        _lightingShader.SetFloat("material.shininess", 32.0f);

        /*
           Here we set all the uniforms for the 5/6 types of lights we have. We have to set them manually and index
           the proper PointLight struct in the array to set each uniform variable. This can be done more code-friendly
           by defining light types as classes and set their values in there, or by using a more efficient uniform approach
           by using 'Uniform buffer objects', but that is something we'll discuss in the 'Advanced GLSL' tutorial.
        */
        
        // Directional light
        UpdateDirectionalLightUniforms(_lightingShader, _directionalLight);

        // Point lights
        foreach (var pointLight in _pointLights)
        {
            UpdatePointLightUniforms(_lightingShader, pointLight);
        }

        // Spot light
        _spotLight.Position = _camera.Position;
        _spotLight.Direction = _camera.Front;
        UpdateSpotLightUniforms(_lightingShader, _spotLight);
        
        // Draw the cubes
        var cubeIndex = 0;
        foreach (var cube in _cubes)
        {
            GL.BindVertexArray(cube.VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, cube.VertexBufferObject);
                
            Matrix4 model = Matrix4.CreateTranslation(cube.Position);
            float angle = 20.0f * cubeIndex;
            model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
            _lightingShader.SetMatrix4("model", model);

            // 36 = 6 sides * 2 triangles * 3 vertices.
            GL.DrawArrays(PrimitiveType.Triangles, 0, cube.IndicesCount);
            
            cubeIndex++;
        }
    }

    
    private void RenderLamps()
    {
        GL.BindVertexArray(_vaoLamp);

        _lampShader.Use();

        _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
        _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
        
        // We use a loop to draw all the lights at the proper position
        GL.BindBuffer(BufferTarget.ArrayBuffer, _lampCube.VertexBufferObject); // All lamps use the same VBO.
        foreach (var pointLight in _pointLights)
        {
            var lampMatrix = Matrix4.CreateScale(0.2f);
            lampMatrix = lampMatrix * Matrix4.CreateTranslation(pointLight.Position);

            _lampShader.SetMatrix4("model", lampMatrix);

            GL.DrawArrays(PrimitiveType.Triangles, 0, _lampCube.IndicesCount);
        }
    }


    private void RenderSkybox()
    {
        GL.Disable(EnableCap.DepthTest);
        GL.Disable(EnableCap.CullFace);
        
        GL.BindVertexArray(_skybox.VertexArrayObject);
        _skyboxTexture.Use(TextureUnit.Texture0);
        _skyboxShader.Use();
        
        // Set the texture unit for the sampler in the fragment shader.
        _skyboxShader.SetInt("material.diffuse", 0);
        
        _skyboxShader.SetMatrix4("view", _camera.GetViewMatrix());
        _skyboxShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _skybox.VertexBufferObject);
        
        _skyboxShader.SetMatrix4("model", Matrix4.CreateTranslation(_camera.Position));

        GL.DrawArrays(PrimitiveType.Triangles, 0, _skybox.IndicesCount);
        
        //GL.Enable(EnableCap.CullFace);
        GL.Enable(EnableCap.DepthTest);
    }


    private void UpdateSpotLightUniforms(Shader lightingShader, SpotLight spotLight)
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

    private void UpdateDirectionalLightUniforms(Shader lightingShader, DirectionalLight directionalLight)
    {
        lightingShader.SetVector3(directionalLight.DirectionUniformName, _directionalLight.Direction);
        lightingShader.SetVector3(directionalLight.AmbientUniformName, _directionalLight.Ambient);
        lightingShader.SetVector3(directionalLight.DiffuseUniformName, _directionalLight.Diffuse);
        lightingShader.SetVector3(directionalLight.SpecularUniformName, _directionalLight.Specular);
    }

    private void UpdatePointLightUniforms(Shader lightingShader, PointLight pointLight)
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