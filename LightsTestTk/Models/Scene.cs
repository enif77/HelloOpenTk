namespace LightsTestTk.Models;

using OpenTK.Mathematics;

using LightsTestTk.Models.SceneObjects;

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

    /// <summary>
    /// A primary camera for the scene.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown, if the value is going to be set to null.</exception>
    public Camera Camera { get; }
    
    /// <summary>
    /// An optional skybox used by this scene.
    /// </summary>
    public Skybox? Skybox { get; set; }
    
    /// <summary>
    /// The maximum number of lights supported by this scene.
    /// </summary>
    public readonly int MaxLights = Defaults.DefaultMaxLights;
    
    /// <summary>
    /// Lights used in this scene.
    /// </summary>
    public readonly IList<ILight> Lights = new List<ILight>();
    
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="camera">A primary scene camera.</param>
    public Scene(Camera camera)
    {
        Camera = camera ?? throw new ArgumentNullException(nameof(camera));
        Camera.Parent = this;
        Children.Add(Camera);
        
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
