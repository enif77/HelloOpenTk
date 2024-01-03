using Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace LightsTestTk.Models.Materials;


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