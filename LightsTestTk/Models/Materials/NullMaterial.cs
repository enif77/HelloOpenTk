namespace LightsTestTk.Models.Materials;

using OpenTK.Mathematics;

using LightsTestTk.Models.Shaders;
using LightsTestTk.Models.Textures;

/// <summary>
/// A material that does nothing. 
/// </summary>
public class NullMaterial : IMaterial
{
    public Vector3 Color => Vector3.Zero;
    public ITexture DiffuseMap { get; } = new NullTexture();
    public ITexture SpecularMap { get; } = new NullTexture();
    public Vector3 Specular { get; set; } = Vector3.Zero;
    public float Shininess { get; set; }
    public IShader Shader { get; } = new NullShader();
}
