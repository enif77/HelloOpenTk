namespace LightsTestTk.Models.Materials;

using Common;
using OpenTK.Mathematics;

/// <summary>
/// A material that uses a single color with no lighting. 
/// </summary>
public class SimpleColorMaterial(Vector3 color, Shader shader) : IMaterial
{
    /// <summary>
    /// A color used by this material.
    /// </summary>
    public Vector3 Color { get; } = color;
    
    /// <summary>
    /// A shader used by this material.
    /// </summary>
    public IShader Shader { get; } = shader ?? throw new ArgumentNullException(nameof(shader));


    /// <summary>
    /// Uses this material for further rendering.
    /// </summary>
    public void Use()
    {
        Shader.Use();
        Shader.SetVector3("color", Color);
    }
}