namespace LightsTestTk.Models.Materials;

using Common;
using OpenTK.Graphics.OpenGL4;

/// <summary>
/// A material that uses a single texture with no lighting. 
/// </summary>
public class SimpleTextureMaterial : IMaterial
{
    /// <summary>
    /// The texture used by this material.
    /// </summary>
    public Texture DiffuseMap { get; }
    
    /// <summary>
    /// The shader used by this material.
    /// </summary>
    public IShader Shader { get; }
    
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="diffuseMap">A texture to be used by this material.</param>
    /// <param name="shader">A shader to be used by this material.</param>
    /// <exception cref="ArgumentNullException">Thrown, when the diffuseMap or the shader parameter is null.</exception>
    public SimpleTextureMaterial(Texture diffuseMap, Shader shader)
    {
        DiffuseMap = diffuseMap ?? throw new ArgumentNullException(nameof(diffuseMap));
        Shader = shader ?? throw new ArgumentNullException(nameof(shader));
    }
    
    
    /// <summary>
    /// Uses this material for further rendering.
    /// </summary>
    public void Use()
    {
        DiffuseMap.Use(TextureUnit.Texture0);
        
        Shader.Use();
        Shader.SetInt("texture0", 0);
    }
}