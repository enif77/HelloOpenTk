namespace LightsTestTk.Models;

using OpenTK.Mathematics;

using LightsTestTk.Models.Materials;


public abstract class SceneObjectBase : ISceneObject
{
    public virtual ISceneObject? Parent { get; set; }
    public IList<ISceneObject> Children { get; } = new List<ISceneObject>();

    public IMaterial Material { get; set; } = new NullMaterial();

    private Vector3 _position = Vector3.Zero;
    public Vector3 Position
    {
        get => _position;

        set
        {
            _position = value;
            NeedsModelMatrixUpdate = true;
        }
    }
    
    private Vector3 _rotation = Vector3.Zero;
    public Vector3 Rotation
    {
        get => _rotation;

        set
        {
            _rotation = value;
            NeedsModelMatrixUpdate = true;
        }
    }
    
    private bool _needsModelMatrixUpdate = true;

    public bool NeedsModelMatrixUpdate
    {
        get => _needsModelMatrixUpdate;

        set
        {
            _needsModelMatrixUpdate = value;
            if (value == false)
            {
                return;
            }

            foreach (var child in Children)
            {
                child.NeedsModelMatrixUpdate = true;
            }
        }
    }
    
    public Matrix4 ModelMatrix { get; set; } = Matrix4.Identity;
    
    
    #region Geometry
    
    public float[] Vertices { get; protected set; } = Array.Empty<float>();
    public int VertexBufferObject { get; set; } = -1;
    public int VertexArrayObject { get; set; } = -1;
    
    #endregion
    
    
    public virtual void Update(float deltaTime)
    {
        if (NeedsModelMatrixUpdate)
        {
            ModelMatrix  = Matrix4.CreateTranslation(Position);
            
            ModelMatrix *= Matrix4.CreateRotationZ(Rotation.Z);
            ModelMatrix *= Matrix4.CreateRotationX(Rotation.X);
            ModelMatrix *= Matrix4.CreateRotationY(Rotation.Y);
            
            NeedsModelMatrixUpdate = false;
        }

        foreach (var child in Children)
        {
            child.Update(deltaTime);
        }
    }

    public virtual void Render()
    {
        foreach (var child in Children)
        {
            child.Render();
        }
    }
}