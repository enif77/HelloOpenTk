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
    /// A material used by this object.
    /// </summary>
    IMaterial Material { get; }
    
    /// <summary>
    /// Position relative to the parent.
    /// </summary>
    Vector3 Position { get; set; }
    
    /// <summary>
    /// Model matrix of the object.
    /// Should be updated before rendering.
    /// </summary>
    Matrix4 ModelMatrix { get; set; }
    
    
    #region Geometry
    
    float[] Vertices { get; }
    int VertexBufferObject { get; set; }
    int VertexArrayObject { get; set; }
    
    #endregion
}