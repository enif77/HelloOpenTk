namespace LightsTestTk.Models.Lights;

using OpenTK.Mathematics;

public class SpotLight : SceneObjectBase, ILight
{
    /// <summary>
    /// The index of the light in the shader.
    /// </summary>
    public int Id { get; }
    
    public bool IsSpotLight { get; }
    public string IsSpotLightUniformName { get; }
    
    // The Position property is inherited from SceneObjectBase class.
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
    
    
    public override void Update(float deltaTime)
    {
        if (NeedsModelMatrixUpdate)
        {
            if (Parent != null)
            {
                // If we have a parent, we are bound to it.
                Position = Parent.Position;
                Rotation = Parent.Rotation;
                
                // This uses the pitch (X) and yaw (Y) angles to calculate the front vector.
                // We ignore the roll angle (Z), because our light is a circle..
                var front = new Vector3
                {
                    X = MathF.Cos(Rotation.X) * MathF.Cos(Rotation.Y),
                    Y = MathF.Sin(Rotation.X),
                    Z = MathF.Cos(Rotation.X) * MathF.Sin(Rotation.Y)
                };
                
                Direction = Vector3.Normalize(front);
            }
            
            ModelMatrix *= Matrix4.CreateRotationZ(Rotation.Z);
            ModelMatrix *= Matrix4.CreateRotationX(Rotation.X);
            ModelMatrix *= Matrix4.CreateRotationY(Rotation.Y);

            ModelMatrix *= Matrix4.CreateTranslation(Position);
            
            // We are already bound to the parent, so we don't need to multiply by the parent's model matrix.
            // if (Parent != null)
            // {
            //     ModelMatrix *= Parent.ModelMatrix;
            // }

            NeedsModelMatrixUpdate = false;
        }

        foreach (var child in Children)
        {
            child.Update(deltaTime);
        }
    }
}