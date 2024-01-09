namespace LightsTestTk.Models.Lights;

using OpenTK.Mathematics;

public class SpotLight : ILight
{
    /// <summary>
    /// The index of the light in the shader.
    /// </summary>
    public int Id { get; }
    
    public bool IsSpotLight { get; }
    public string IsSpotLightUniformName { get; }
    
    public Vector3 Position { get; set; }
    public string PositionUniformName { get; }
    
    public Vector3 Direction { get; set; }
    public string DirectionUniformName { get; }
    
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
    
    public float CutOff { get; set; }
    public string CutOffUniformName { get; }
    
    public float OuterCutOff { get; set; }
    public string OuterCutOffUniformName { get; }
    
    
    public SpotLight(int id, bool isSpotLight = false)
    {
        Id = id;
        
        IsSpotLight = isSpotLight;
        IsSpotLightUniformName = $"pointLights[{Id}].isSpotLight";
        
        Position = new Vector3();
        PositionUniformName = $"pointLights[{Id}].position";
        
        Direction = -Vector3.UnitZ;
        DirectionUniformName = $"pointLights[{Id}].direction";
        
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
        
        CutOff = MathF.Cos(MathHelper.DegreesToRadians(12.5f));
        CutOffUniformName = $"pointLights[{Id}].cutOff";
        
        OuterCutOff = MathF.Cos(MathHelper.DegreesToRadians(17.5f));
        OuterCutOffUniformName = $"pointLights[{Id}].outerCutOff";
    }
}