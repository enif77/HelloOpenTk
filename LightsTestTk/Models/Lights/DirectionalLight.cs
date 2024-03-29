namespace LightsTestTk.Models.Lights;

using OpenTK.Mathematics;

public class DirectionalLight : SceneObjectBase, ILight
{
    public int Id { get; }
    
    public LightType LightType => LightType.Directional;
    public string LightTypeUniformName { get; }
    
    public Vector3 Direction { get; set; }
    private string DirectionUniformName { get; }

    public Vector3 Ambient { get; set; }
    private string AmbientUniformName { get; }
    
    public Vector3 Diffuse { get; set; }
    private string DiffuseUniformName { get; }
    
    public Vector3 Specular { get; set; }
    private string SpecularUniformName { get; }
    
    
    public DirectionalLight(int id)
    {
        Id = id;
        
        LightTypeUniformName = $"lights[{Id}].lightType";
        
        Direction = new Vector3(0.2f, -1.0f, -0.3f);
        DirectionUniformName = $"lights[{Id}].direction";
        
        Ambient = new Vector3(0.05f, 0.05f, 0.05f);
        AmbientUniformName = $"lights[{Id}].ambient";
        
        Diffuse = new Vector3(0.4f, 0.4f, 0.4f);
        DiffuseUniformName = $"lights[{Id}].diffuse";
        
        Specular = new Vector3(0.5f, 0.5f, 0.5f);
        SpecularUniformName = $"lights[{Id}].specular";
    }
    
    
    public void SetUniforms(Shader shader)
    {
        shader.SetInt(LightTypeUniformName, (int)LightType);
        
        shader.SetVector3(DirectionUniformName, Direction);
        shader.SetVector3(AmbientUniformName, Ambient);
        shader.SetVector3(DiffuseUniformName, Diffuse);
        shader.SetVector3(SpecularUniformName, Specular);
    }
}