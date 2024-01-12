using LightsTestTk.Models.Lights;
using OpenTK.Mathematics;

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
    /// Creates and adds a point light to the scene.
    /// </summary>
    /// <param name="scene">A scene.</param>
    /// <param name="position">An optional position of the new light.</param>
    /// <returns>A newly created point light.</returns>
    /// <exception cref="InvalidOperationException">If the MaxPointLights point lights are already in this scene.</exception>
    public static SpotLight CreatePointLight(this Scene scene, Vector3 position = default)
    {
        if (scene.PointLights.Count >= scene.MaxPointLights)
        {
            throw new InvalidOperationException($"Only {scene.MaxPointLights} point lights are supported.");
        }
        
        var pointLight = new SpotLight(scene.PointLights.Count)
        {
            Position = position
        };
        
        scene.PointLights.Add(pointLight);
        
        return pointLight;
    }
    
    /// <summary>
    /// Creates and adds a spot light to the scene.
    /// </summary>
    /// <param name="scene">A scene.</param>
    /// <param name="position">An optional position of the new light.</param>
    /// <returns>A newly created point light.</returns>
    /// <exception cref="InvalidOperationException">If the MaxPointLights point lights are already in this scene.</exception>
    public static SpotLight CreateSpotLight(this Scene scene, Vector3 position = default)
    {
        if (scene.PointLights.Count >= scene.MaxPointLights)
        {
            throw new InvalidOperationException($"Only {scene.MaxPointLights} point lights are supported.");
        }
        
        var pointLight = new SpotLight(scene.PointLights.Count, true)
        {
            Position = position,
            Ambient = new Vector3(0.0f, 0.0f, 0.0f),
            Diffuse = new Vector3(1.0f, 1.0f, 1.0f),
            Specular = new Vector3(1.0f, 1.0f, 1.0f),
            Constant = 1.0f,
            Linear = 0.09f,
            Quadratic = 0.032f,
            CutOff = MathF.Cos(MathHelper.DegreesToRadians(12.5f)),
            OuterCutOff = MathF.Cos(MathHelper.DegreesToRadians(17.5f))
        };
        
        scene.PointLights.Add(pointLight);
        
        return pointLight;
    }

    // /// <summary>
    // /// Adds a shader to the scene.
    // /// </summary>
    // /// <param name="scene">A scene.</param>
    // /// <param name="shader">An IShader instance.</param>
    // /// <exception cref="InvalidOperationException">If a shader with the shader.Name name already exists in the shaders collection.</exception>
    // /// <exception cref="ArgumentNullException">If the shader is null.</exception>
    // public static void AddShader(this Scene scene, IShader shader)
    // {
    //     ArgumentNullException.ThrowIfNull(shader);
    //     if (scene.Shaders.TryAdd(shader.Name, shader) == false)
    //     {
    //         throw new InvalidOperationException($"Shader with the '{shader.Name}' name already exists.");
    //     }
    // }
}