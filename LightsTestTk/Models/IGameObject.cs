namespace LightsTestTk.Models;

using OpenTK.Mathematics;

/// <summary>
/// Generic interface for all game objects.
/// </summary>
public interface IGameObject
{
    /// <summary>
    /// This object's parent.
    /// </summary>
    IGameObject? Parent { get; set; }
    
    /// <summary>
    /// This game object's children.
    /// </summary>
    IList<IGameObject> Children { get; }
    
    /// <summary>
    /// Position relative to the parent.
    /// </summary>
    Vector3 Position { get; set; }
}