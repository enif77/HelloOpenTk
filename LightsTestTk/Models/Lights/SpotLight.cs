namespace LightsTestTk.Models.Lights;

using OpenTK.Mathematics;

public class SpotLight : PointLight
{
    public bool IsSpotLight { get; }
    public string IsSpotLightUniformName { get; }
    
    public Vector3 Direction { get; set; }
    public string DirectionUniformName { get; }
    
    public float CutOff { get; set; }
    public string CutOffUniformName { get; }
    
    public float OuterCutOff { get; set; }
    public string OuterCutOffUniformName { get; }
    
    
    public SpotLight(int id, bool isSpotLight = false)
        : base(id)
    {
        IsSpotLight = isSpotLight;
        IsSpotLightUniformName = $"pointLights[{Id}].isSpotLight";
       
        Direction = -Vector3.UnitZ;
        DirectionUniformName = $"pointLights[{Id}].direction";
        
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