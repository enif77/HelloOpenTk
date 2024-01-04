using LightsTestTk.Models.Lights;

namespace LightsTestTk.Models;

using OpenTK.Mathematics;

using Common;

/// <summary>
/// Scene is the parent for all game objects.
/// </summary>
public class Scene : IGameObject
{
    /// <summary>
    /// Scene cannot have a parent.
    /// </summary>
    /// <exception cref="InvalidOperationException">When a scene parent is going to be set.</exception>
    public IGameObject? Parent
    {
        get => null;
        set => throw new InvalidOperationException("Scene cannot have a parent.");
    }
    
    /// <summary>
    /// All children of the scene.
    /// </summary>
    public IList<IGameObject> Children { get; }
    
    /// <summary>
    /// Unused scene position.
    /// </summary>
    public Vector3 Position { get; set; }
    
    private Camera _camera;

    /// <summary>
    /// A primary camera for the scene.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown, if the value is going to be set to null.</exception>
    public Camera Camera
    {
        get => _camera;
        
        set => _camera = value ?? throw new InvalidOperationException("Scene must have a camera.");
    }


    public Skybox? Skybox { get; set; }

    public Cube LampCube = new Cube(-1);
    
    public DirectionalLight DirectionalLight = new DirectionalLight();
    public SpotLight SpotLight = new SpotLight();
    
    // We need the point lights' positions to draw the lamps and to get light the materials properly
    public PointLight[] PointLights =
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
    
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="camera">A primary scene camera.</param>
    public Scene(Camera camera)
    {
        Camera = camera;
        Children = new List<IGameObject>();
    }
}
