namespace LightsTestTk.Models;

/// <summary>
/// Defines an object, that can be updated over time.
/// </summary>
public interface IUpdateable
{
    /// <summary>
    /// Updates a game object.
    /// </summary>
    /// <param name="deltaTime">How much time passed since the last call of this method.</param>
    /// <returns>True, if this method should be called again. False if no more updates are required.</returns>
    bool Update(float deltaTime);
}