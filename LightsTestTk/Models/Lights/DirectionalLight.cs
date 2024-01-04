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
    
    /// <summary>
    /// Position relative to the parent.
    /// NOTE: This is not used for directional lights.
    /// </summary>
    public Vector3 Position { get; set; }
    
    public Vector3 Direction { get; set; }
    public string DirectionUniformName { get; }

    public Vector3 Ambient { get; set; }
    public string AmbientUniformName { get; }
    
    public Vector3 Diffuse { get; set; }
    public string DiffuseUniformName { get; }
    
    public Vector3 Specular { get; set; }
    public string SpecularUniformName { get; }
    
    
    public DirectionalLight()
    {
        Id = 0;
        
        Position = new Vector3();
        
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