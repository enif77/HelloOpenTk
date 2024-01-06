namespace LightsTestTk.Models.Materials;

using OpenTK.Mathematics;

using Common;

/// <summary>
/// A material that uses a single color with no lighting. 
/// </summary>
/// <param name="color">A color used by this material.</param>
public class SimpleColorMaterial(Vector3 color) : IMaterial
{
    public Vector3 Color { get; } = color;
    public ITexture DiffuseMap { get; } = new NullTexture();
    public ITexture SpecularMap { get; } = new NullTexture();
    public Vector3 Specular { get; set; } = Vector3.Zero;
    public float Shininess { get; set; }
}