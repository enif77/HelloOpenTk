namespace LightsTestTk.Models.Lights;

using OpenTK.Mathematics;

using LightsTestTk.Models.Materials;

public class PointLight : ILight
{
    /// <summary>
    /// The index of the light in the shader.
    /// </summary>
    public int Id { get; }
    
    public IGameObject? Parent { get; set; }
    public IList<IGameObject> Children { get; }
    public IMaterial Material { get; }

    public Vector3 Position { get; set; }
    public Matrix4 ModelMatrix { get; set; }
    public string PositionUniformName { get; }
    
    public Vector3 Ambient { get; set; }
    public string AmbientUniformName { get; }
    
    public Vector3 Diffuse { get; set; }
    public string DiffuseUniformName { get; }
    
    public Vector3 Specular { get; set; }
    public string SpecularUniformName { get; }
    
    public float Constant { get; set; }
    public string ConstantUniformName { get; }
    
    public float Linear { get; set; }
    public string LinearUniformName { get; }
    
    public float Quadratic { get; set; }
    public string QuadraticUniformName { get; }
    
    
    #region Geometry

    public float[] Vertices { get; } = Array.Empty<float>();
    public int VertexBufferObject { get; set; } = -1;
    public int VertexArrayObject { get; set; } = -1;
    
    #endregion
    
    
    public PointLight(int id)
    {
        Id = id;
        
        Material = new NullMaterial();
        ModelMatrix = Matrix4.Identity;
        
        Position = new Vector3();
        PositionUniformName = $"pointLights[{Id}].position";
        
        Ambient = new Vector3(0.05f, 0.05f, 0.05f);
        AmbientUniformName = $"pointLights[{Id}].ambient";
        
        Diffuse = new Vector3(0.8f, 0.8f, 0.8f);
        DiffuseUniformName = $"pointLights[{Id}].diffuse";
        
        Specular = new Vector3(1.0f, 1.0f, 1.0f);
        SpecularUniformName = $"pointLights[{Id}].specular";
        
        Constant = 1.0f;
        ConstantUniformName = $"pointLights[{Id}].constant";
        
        Linear = 0.09f;
        LinearUniformName = $"pointLights[{Id}].linear";
        
        Quadratic = 0.032f;
        QuadraticUniformName = $"pointLights[{Id}].quadratic";
        
        Children = new List<IGameObject>();
    }
}