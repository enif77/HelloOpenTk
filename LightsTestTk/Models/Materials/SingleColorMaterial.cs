namespace LightsTestTk.Models.Materials;

using Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

/// <summary>
/// A material that uses a single color with no lighting. 
/// </summary>
public class SingleColorMaterial : IMaterial
{
    public Vector3 Color { get; }
    public IShader Shader { get; }
    
    
    public SingleColorMaterial(Vector3 color, Shader shader)
    {
        Color = color;
        Shader = shader ?? throw new ArgumentNullException(nameof(shader));
    }
    
    
    public void Use()
    {
        Shader.Use();
    }
}