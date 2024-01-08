namespace LightsTestTk.Extensions;

using LightsTestTk.Models;

/// <summary>
/// Scene related extensions.
/// </summary>
public static class SceneExtensions
{
    /// <summary>
    /// Adds a child to a game object.
    /// </summary>
    /// <param name="scene">A scene.</param>
    /// <param name="skybox">A skybox to be added to a scene.</param>
    /// <exception cref="InvalidOperationException">Thrown, when such child already exists in game object children.</exception>
    public static void AddSkybox(this Scene scene, Skybox skybox)
    {
        if (skybox == null) throw new ArgumentNullException(nameof(skybox));
        
        skybox.Parent = scene;
        scene.Skybox = skybox;
    }


    /// <summary>
    /// Adds a shader to the scene.
    /// </summary>
    /// <param name="scene">A scene.</param>
    /// <param name="shader">An IShader instance.</param>
    /// <exception cref="InvalidOperationException">If a shader with the shader.Name name already exists in the shaders collection.</exception>
    /// <exception cref="ArgumentNullException">If the shader is null.</exception>
    public static void AddShader(this Scene scene, IShader shader)
    {
        ArgumentNullException.ThrowIfNull(shader);
        if (scene.Shaders.TryAdd(shader.Name, shader) == false)
        {
            throw new InvalidOperationException($"Shader with the '{shader.Name}' name already exists.");
        }
    }
}