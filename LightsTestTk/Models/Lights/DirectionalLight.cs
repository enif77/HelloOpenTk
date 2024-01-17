namespace LightsTestTk.Models.Lights;

using OpenTK.Mathematics;

public class DirectionalLight : SceneObjectBase, ILight
{
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
    
    
    public DirectionalLight()
    {
        LightTypeUniformName = "directionalLight.lightType";
        
        Direction = new Vector3(0.2f, -1.0f, -0.3f);
        DirectionUniformName = "directionalLight.direction";
        
        Ambient = new Vector3(0.05f, 0.05f, 0.05f);
        AmbientUniformName = "directionalLight.ambient";
        
        Diffuse = new Vector3(0.4f, 0.4f, 0.4f);
        DiffuseUniformName = "directionalLight.diffuse";
        
        Specular = new Vector3(0.5f, 0.5f, 0.5f);
        SpecularUniformName = "directionalLight.specular";
    }
    
    
    public void SetUniforms(Shader shader)
    {
        shader.SetVector3(DirectionUniformName, Direction);
        shader.SetVector3(AmbientUniformName, Ambient);
        shader.SetVector3(DiffuseUniformName, Diffuse);
        shader.SetVector3(SpecularUniformName, Specular);
    }
}