using Common;
using LightsTestTk.Extensions;
using LightsTestTk.Models;
using LightsTestTk.Models.Lights;
using LightsTestTk.Models.Materials;
using LightsTestTk.Models.Shaders;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LightsTestTk;

public class Game
{
    private Scene? _scene;
    
    private SpotLight? _spotLight;
    private readonly List<Cube> _cubes = new();
    
    
    public Camera Camera => _scene?.Camera ?? throw new InvalidOperationException("The scene is not initialized.");
    
    
    public bool Initialize(int width, int height)
    {
        var scene = new Scene(
            new Camera(Vector3.UnitZ * 3, width / (float)height));
        
        #region Shaders
        
        scene.AddShader(new NullShader());
        scene.AddShader(new SkyboxShader());
        scene.AddShader(new LampShader());
        scene.AddShader(new DefaultShader());
        
        #endregion
        
        
        #region Skybox
        
        var skybox = new Skybox(
            new SimpleTextureMaterial(
                Texture.LoadFromFile("Resources/Textures/SKYBOX.jpg"),
                scene.Shaders["skybox"]));
        
        skybox.GenerateVertexObjectBuffer();
        skybox.GenerateVertexArrayObjectForPosTexVbo();
        
        scene.AddSkybox(skybox);
        
        #endregion
        
        
        #region Cubes
        
        // TODO: Create the first cube and then clone it with setting position to cube clones.
        
        var cubeMaterial = new Material(
            Texture.LoadFromFile("Resources/Textures/container2.png"),
            Texture.LoadFromFile("Resources/Textures/container2_specular.png"),
            scene.Shaders["cube"]);
        
        scene.AddChild(CreateCube(cubeMaterial, new Vector3(0.0f, 0.0f, 0.0f)));
        scene.AddChild(CreateCube(cubeMaterial, new Vector3(2.0f, 5.0f, -15.0f)));
        scene.AddChild(CreateCube(cubeMaterial, new Vector3(-1.5f, -2.2f, -2.5f)));
        scene.AddChild(CreateCube(cubeMaterial, new Vector3(-3.8f, -2.0f, -12.3f)));
        scene.AddChild(CreateCube(cubeMaterial, new Vector3(2.4f, -0.4f, -3.5f)));
        scene.AddChild(CreateCube(cubeMaterial, new Vector3(-1.7f, 3.0f, -7.5f)));
        scene.AddChild(CreateCube(cubeMaterial, new Vector3(1.3f, -2.0f, -2.5f)));
        scene.AddChild(CreateCube(cubeMaterial, new Vector3(1.5f, 2.0f, -2.5f)));
        scene.AddChild(CreateCube(cubeMaterial, new Vector3(1.5f, 0.2f, -1.5f)));
        scene.AddChild(CreateCube(cubeMaterial, new Vector3(-1.3f, 1.0f, -1.5f)));
        
        #endregion
        
        
        #region Lamps
        
        scene.CreatePointLight(new Vector3(0.7f, 0.2f, 2.0f));
        scene.CreatePointLight(new Vector3(2.3f, -3.3f, -4.0f));
        scene.CreatePointLight(new Vector3(-4.0f, 2.0f, -12.0f));
        var redLight = scene.CreatePointLight(new Vector3(0.0f, 0.0f, -3.0f));
        redLight.Diffuse = new Vector3(1.0f, 0.0f, 0.0f);
        
        var lampMaterial = new SimpleColorMaterial(
            new Vector3(1.0f, 1.0f, 1.0f),
            scene.Shaders["lamp"]);
        
        var redLampMaterial = new SimpleColorMaterial(
            new Vector3(1.0f, 0.0f, 0.0f),
            scene.Shaders["lamp"]);

        var lampId = 0;
        foreach (var pointLight in scene.PointLights)
        {
            var lamp = new Cube()
            {
                Material = (lampId == 3) ? redLampMaterial : lampMaterial,
                Position = pointLight.Position
            };
        
            lamp.GenerateVertexObjectBuffer();
            lamp.GenerateVertexArrayObjectForPosNormTexVbo();
            
            _cubes.Add(lamp);
            
            scene.AddChild(lamp);
            
            lampId++;
        }
        
        _spotLight = scene.CreateSpotLight(new Vector3(0.7f, 0.2f, 2.0f));
        _spotLight.Diffuse = new Vector3(0.0f, 1.0f, 0.0f);
        
        #endregion
        
        _scene = scene;
        
        return true;
    }
    
    
    private bool _firstMove = true;
    private Vector2 _lastPos;
    
    public bool Update(float deltaTime, KeyboardState keyboardState, MouseState mouseState)
    {
        if (_scene == null)
        {
            throw new InvalidOperationException("The scene is not initialized.");
        }
     
        if (keyboardState.IsKeyDown(Keys.Escape))
        {
            return false;
        }
        
        
        const float cameraSpeed = 1.5f;
        const float sensitivity = 0.2f;

        if (keyboardState.IsKeyDown(Keys.W))
        {
            _scene.Camera.Position += _scene.Camera.Front * cameraSpeed * deltaTime; // Forward
        }
        if (keyboardState.IsKeyDown(Keys.S))
        {
            _scene.Camera.Position -= _scene.Camera.Front * cameraSpeed * deltaTime; // Backwards
        }
        if (keyboardState.IsKeyDown(Keys.A))
        {
            _scene.Camera.Position -= _scene.Camera.Right * cameraSpeed * deltaTime; // Left
        }
        if (keyboardState.IsKeyDown(Keys.D))
        {
            _scene.Camera.Position += _scene.Camera.Right * cameraSpeed * deltaTime; // Right
        }
        if (keyboardState.IsKeyDown(Keys.Space))
        {
            _scene.Camera.Position += _scene.Camera.Up * cameraSpeed * deltaTime; // Up
        }
        if (keyboardState.IsKeyDown(Keys.LeftShift))
        {
            _scene.Camera.Position -= _scene.Camera.Up * cameraSpeed * deltaTime; // Down
        }

        var mouse = mouseState;

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

            _scene.Camera.Yaw += deltaX * sensitivity;
            _scene.Camera.Pitch -= deltaY * sensitivity;
        }
        
        
        // Update the spot light bound to the camera.
        _spotLight.Position = _scene.Camera.Position;
        _spotLight.Direction = _scene.Camera.Front;
        
        var cubeIndex = 0;
        foreach (var cube in _cubes)
        {
            var model = Matrix4.CreateTranslation(cube.Position);
            
            if (cube.Material is SimpleColorMaterial)
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
        
        return true;
    }
    
    
    public void Render()
    {
        if (_scene == null)
        {
            throw new InvalidOperationException("The scene is not initialized.");
        }

        _scene.Render();
    }
    
    
    private Cube CreateCube(IMaterial material, Vector3 position)
    {
        var cube = new Cube()
        {
            Material = material,
            Position = position
        };
        
        cube.GenerateVertexObjectBuffer();
        cube.GenerateVertexArrayObjectForPosNormTexVbo();
        
        _cubes.Add(cube);
        
        return cube;
    }
}