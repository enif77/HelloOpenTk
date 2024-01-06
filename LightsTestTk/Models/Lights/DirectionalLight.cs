using LightsTestTk.Models.Materials;

namespace LightsTestTk.Models.Lights;

using OpenTK.Mathematics;

public class DirectionalLight : ILight
{
    /// <summary>
    /// The index of the light in the shader.
    /// </summary>
    public int Id { get; }
    
    public IGameObject? Parent { get; set; }
    public IList<IGameObject> Children { get; }
    
    public IMaterial Material { get; }
    
    /// <summary>
    /// Position relative to the parent.
    /// NOTE: This is not used for directional lights.
    /// </summary>
    public Vector3 Position { get; set; }

    public Matrix4 ModelMatrix { get; set; }

    public Vector3 Direction { get; set; }
    public string DirectionUniformName { get; }

    public Vector3 Ambient { get; set; }
    public string AmbientUniformName { get; }
    
    public Vector3 Diffuse { get; set; }
    public string DiffuseUniformName { get; }
    
    public Vector3 Specular { get; set; }
    public string SpecularUniformName { get; }
    
    
    #region Geometry

    public float[] Vertices { get; } = Array.Empty<float>();
    public int VertexBufferObject { get; set; } = -1;
    public int VertexArrayObject { get; set; } = -1;
    
    #endregion
    
    
    public DirectionalLight()
    {
        Id = 0;
        
        Position = new Vector3();
        
        Material = new NullMaterial();
        ModelMatrix = Matrix4.Identity;
        
        Direction = new Vector3(0.2f, -1.0f, -0.3f);
        DirectionUniformName = $"directionalLight.direction";
        
        Ambient = new Vector3(0.05f, 0.05f, 0.05f);
        AmbientUniformName = $"directionalLight.ambient";
        
        Diffuse = new Vector3(0.4f, 0.4f, 0.4f);
        DiffuseUniformName = $"directionalLight.diffuse";
        
        Specular = new Vector3(0.5f, 0.5f, 0.5f);
        SpecularUniformName = $"directionalLight.specular";
        
        Children = new List<IGameObject>();
    }
}