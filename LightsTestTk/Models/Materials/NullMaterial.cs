using Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace LightsTestTk.Models.Materials;


/// <summary>
/// A material that does nothing. 
/// </summary>
public class NullMaterial : IMaterial
{
    public Shader Shader { get; }
    
    
    public NullMaterial()
    {
        Shader = null;  // TODO: Create a shader for this.
    }
    
    
    public void Use()
    {
        // Do nothing.
    }
}