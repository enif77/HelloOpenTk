namespace LightsTestTk.Models;

/// <summary>
/// Defines a shader used for rendering of an object.
/// </summary>
public interface IShader
{
    string Name { get; }
    
    int GetAttributeLocation(string name);
    
    void Use(Scene scene, IGameObject gameObject);
}