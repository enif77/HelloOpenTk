namespace LightsTestTk.Models;

public interface IRenderable
{
    /// <summary>
    /// Material used to render this object.
    /// </summary>
    IMaterial Material { get; }
    
    
    /// <summary>
    /// Renders this object.
    /// </summary>
    void Render();
}