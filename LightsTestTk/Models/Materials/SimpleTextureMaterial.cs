namespace LightsTestTk.Models.Materials;

using OpenTK.Mathematics;

using Common;

/// <summary>
/// A material that uses a single texture with no lighting. 
/// </summary>
public class SimpleTextureMaterial : IMaterial
{
    public Vector3 Color => new(1);
    
    /// <summary>
    /// The texture used by this material.
    /// </summary>
    public ITexture DiffuseMap { get; }

    public ITexture SpecularMap { get; } = new NullTexture();
    public Vector3 Specular { get; set; } = Vector3.Zero;
    public float Shininess { get; set; }
    
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="diffuseMap">A texture to be used by this material.</param>
    /// <exception cref="ArgumentNullException">Thrown, when the diffuseMap or the shader parameter is null.</exception>
    public SimpleTextureMaterial(Texture diffuseMap)
    {
        DiffuseMap = diffuseMap ?? throw new ArgumentNullException(nameof(diffuseMap));
    }
}