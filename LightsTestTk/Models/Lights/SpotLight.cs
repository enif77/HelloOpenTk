using LightsTestTk.Models.Materials;

namespace LightsTestTk.Models.Lights;

using OpenTK.Mathematics;

public class SpotLight : ILight
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


    #region Geometry

    public float[] Vertices { get; } = Array.Empty<float>();
    public int VertexBufferObject { get; set; } = -1;
    public int VertexArrayObject { get; set; } = -1;
    
    #endregion
    
    
    public SpotLight()
    {
        Id = 0;
        
        Material = new NullMaterial();
        ModelMatrix = Matrix4.Identity;
        
        Position = new Vector3();
        PositionUniformName = $"spotLight.position";
        
        Direction = -Vector3.UnitZ;
        DirectionUniformName = $"spotLight.direction";
        
        Ambient = new Vector3(0.0f, 0.0f, 0.0f);
        AmbientUniformName = $"spotLight.ambient";
        
        Diffuse = new Vector3(1.0f, 1.0f, 1.0f);
        DiffuseUniformName = $"spotLight.diffuse";
        
        Specular = new Vector3(1.0f, 1.0f, 1.0f);
        SpecularUniformName = $"spotLight.specular";
        
        Constant = 1.0f;
        ConstantUniformName = $"spotLight.constant";
        
        Linear = 0.09f;
        LinearUniformName = $"spotLight.linear";
        
        Quadratic = 0.032f;
        QuadraticUniformName = $"spotLight.quadratic";
        
        CutOff = MathF.Cos(MathHelper.DegreesToRadians(12.5f));
        CutOffUniformName = $"spotLight.cutOff";
        
        OuterCutOff = MathF.Cos(MathHelper.DegreesToRadians(17.5f));
        OuterCutOffUniformName = $"spotLight.outerCutOff";
        
        Children = new List<IGameObject>();
    }
}