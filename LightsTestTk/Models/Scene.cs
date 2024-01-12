namespace LightsTestTk.Models;

using OpenTK.Mathematics;

using LightsTestTk.Models.Lights;

/// <summary>
/// Scene is the parent for all game objects.
/// </summary>
public class Scene : SceneObjectBase
{
    /// <summary>
    /// Scene cannot have a parent.
    /// </summary>
    /// <exception cref="InvalidOperationException">When a scene parent is going to be set.</exception>
    public override ISceneObject? Parent
    {
        get => null;
        set => throw new InvalidOperationException("Scene cannot have a parent.");
    }

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
    /// An optional skybox used by this scene.
    /// </summary>
    public Skybox? Skybox { get; set; }
    
    /// <summary>
    /// A directional light used by this scene.
    /// </summary>
    public readonly DirectionalLight DirectionalLight = new DirectionalLight();
    
    /// <summary>
    /// The maximum number of point lights supported by this scene.
    /// </summary>
    public readonly int MaxPointLights = Defaults.DefaultMaxPointLights;
    
    /// <summary>
    /// Point lights used by this scene.
    /// </summary>
    public readonly IList<SpotLight> PointLights = new List<SpotLight>();
    
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="camera">A primary scene camera.</param>
    public Scene(Camera camera)
    {
        Camera = camera ?? throw new ArgumentNullException(nameof(camera));
        ModelMatrix = Matrix4.Identity;
    }


    public override void Update(float deltaTime)
    {
        Skybox?.Update(deltaTime);
        base.Update(deltaTime);
    }


    public override void Render()
    {
        Skybox?.Render();
        base.Render();
    }
}
