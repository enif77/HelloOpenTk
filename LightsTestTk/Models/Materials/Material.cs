namespace LightsTestTk.Models.Materials;

using Common;
using OpenTK.Mathematics;

/// <summary>
/// A material that uses multiple textures with lighting. 
/// </summary>
public class Material : IMaterial
{
    public Vector3 Color => new(1);
    public ITexture DiffuseMap { get; }
    public ITexture SpecularMap { get; }
    public Vector3 Specular { get; set; }
    public float Shininess { get; set; }

    
    public Material(ITexture diffuseMap, ITexture specularMap)
    {
        DiffuseMap = diffuseMap ?? throw new ArgumentNullException(nameof(diffuseMap));
        SpecularMap = specularMap ?? throw new ArgumentNullException(nameof(specularMap));
        Specular = new Vector3(0.5f, 0.5f, 0.5f);
        Shininess = 32.0f;
    }
}