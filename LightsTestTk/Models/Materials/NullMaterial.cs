namespace LightsTestTk.Models.Materials;

using Common;

/// <summary>
/// A material that does nothing. 
/// </summary>
public class NullMaterial : IMaterial
{
    public IShader Shader { get; }
    
    
    public NullMaterial()
    {
        Shader = new NullShader();
    }
    
    
    public void Use()
    {
        // Do nothing.
    }
}