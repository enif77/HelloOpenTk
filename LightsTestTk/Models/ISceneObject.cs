namespace LightsTestTk.Models;

using OpenTK.Mathematics;

/// <summary>
/// Generic interface for all scene objects.
/// </summary>
public interface ISceneObject : IUpdatable, IRenderable
{
    /// <summary>
    /// This scene object's parent.
    /// </summary>
    ISceneObject? Parent { get; set; }
    
    /// <summary>
    /// This scene object's children.
    /// </summary>
    IList<ISceneObject> Children { get; }
    
    /// <summary>
    /// A material used by this scene object.
    /// </summary>
    IMaterial Material { get; }
    
    /// <summary>
    /// Position of this scene object relative to the parent.
    /// </summary>
    Vector3 Position { get; set; }
    
    /// <summary>
    /// Model matrix of tis scene object.
    /// Should be updated before rendering.
    /// </summary>
    Matrix4 ModelMatrix { get; set; }
    
    
    #region Geometry
    
    float[] Vertices { get; }
    int VertexBufferObject { get; set; }
    int VertexArrayObject { get; set; }
    
    #endregion
}