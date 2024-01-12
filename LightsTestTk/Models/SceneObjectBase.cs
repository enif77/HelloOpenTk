namespace LightsTestTk.Models;

using OpenTK.Mathematics;

using LightsTestTk.Models.Materials;


public abstract class SceneObjectBase : ISceneObject
{
    public virtual ISceneObject? Parent { get; set; }
    public IList<ISceneObject> Children { get; } = new List<ISceneObject>();

    public IMaterial Material { get; set; } = new NullMaterial();
    public Vector3 Position { get; set; } = Vector3.Zero;
    public Matrix4 ModelMatrix { get; set; } = Matrix4.Identity;
    
    
    #region Geometry
    
    public float[] Vertices { get; protected set; } = Array.Empty<float>();
    public int VertexBufferObject { get; set; } = -1;
    public int VertexArrayObject { get; set; } = -1;
    
    #endregion
    
    
    public virtual void Update(float deltaTime)
    {
        foreach (var child in Children)
        {
            if (child is not IUpdatable updatableChild)
            {
                continue;
            }

            updatableChild.Update(deltaTime);
        }
    }

    public virtual void Render()
    {
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