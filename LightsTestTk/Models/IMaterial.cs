using OpenTK.Mathematics;

namespace LightsTestTk.Models;

using Common;

/// <summary>
/// Defines a material used for rendering of an object.
/// </summary>
public interface IMaterial
{
    /// <summary>
    /// A color used by this material.
    /// </summary>
    Vector3 Color { get; }
    
    /// <summary>
    /// The texture used by this material.
    /// </summary>
    ITexture DiffuseMap { get; }
    
    /// <summary>
    /// A specular map used by this material.
    /// </summary>
    ITexture SpecularMap { get; }
    
    /// <summary>
    /// Specular color of this material.
    /// </summary>
    Vector3 Specular { get; set; }
    
    /// <summary>
    /// Shininess of this material.
    /// </summary>
    float Shininess { get; set; }
}