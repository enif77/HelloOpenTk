namespace LightsTestTk.Models;

using OpenTK.Mathematics;
using LightsTestTk.Models.Lights;
using LightsTestTk.Models.Materials;

/// <summary>
/// Scene is the parent for all game objects.
/// </summary>
public class Scene : ISceneObject, IRenderable
{
    /// <summary>
    /// Scene cannot have a parent.
    /// </summary>
    /// <exception cref="InvalidOperationException">When a scene parent is going to be set.</exception>
    public ISceneObject? Parent
    {
        get => null;
        set => throw new InvalidOperationException("Scene cannot have a parent.");
    }
    
    /// <summary>
    /// All children of the scene.
    /// </summary>
    public IList<ISceneObject> Children { get; }

    public IMaterial Material { get; }

    /// <summary>
    /// Unused scene position.
    /// </summary>
    public Vector3 Position { get; set; }

    public Matrix4 ModelMatrix { get; set; }

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


    /// <summary>
    /// All known shaders used in the scene.
    /// </summary>
    public readonly Dictionary<string, IShader> Shaders = new();
    
    public Skybox? Skybox { get; set; }
    
    public DirectionalLight DirectionalLight = new DirectionalLight();
    
    public readonly int MaxPointLights = Defaults.DefaultMaxPointLights;
    public readonly IList<SpotLight> PointLights = new List<SpotLight>();
    
    
    #region Geometry

    public float[] Vertices { get; } = Array.Empty<float>();
    public int VertexBufferObject { get; set; } = -1;
    public int VertexArrayObject { get; set; } = -1;
    
    #endregion
    
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="camera">A primary scene camera.</param>
    public Scene(Camera camera)
    {
        Camera = camera ?? throw new ArgumentNullException(nameof(camera));
        Material = new NullMaterial();
        ModelMatrix = Matrix4.Identity;
        Children = new List<ISceneObject>();
    }

    
    public void Render()
    {
        Skybox?.Render();

        foreach (var child in Children)
        {
            if (child is not IRenderable renderableChild)
            {
                continue;
            }

            renderableChild.Render();
        }
    }
}
