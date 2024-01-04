namespace LightsTestTk.Extensions;

using LightsTestTk.Models;

public static class GameObjectExtensions
{
    /// <summary>
    /// Try to get a scene from the game object.
    /// </summary>
    /// <param name="gameObject">A game object.</param>
    /// <returns>A Scene instance a game object belongs to.</returns>
    /// <exception cref="InvalidOperationException">When no scene object was found as a parent of a game object.</exception>
    public static Scene GetScene(this IGameObject gameObject)
    {
        while (true)
        {
            if (gameObject is Scene scene)
            {
                return scene;
            }

            gameObject = gameObject.Parent ?? throw new InvalidOperationException("Scene not found.");
        }
    }

    /// <summary>
    /// Adds a child to a game object.
    /// </summary>
    /// <param name="gameObject">A game object.</param>
    /// <param name="child">A child to be added.</param>
    /// <exception cref="InvalidOperationException">Thrown, when such child already exists in game object children.</exception>
    public static void AddChild(this IGameObject gameObject, IGameObject child)
    {
        if (child == null) throw new ArgumentNullException(nameof(child));
        child.Parent = gameObject ?? throw new ArgumentNullException(nameof(gameObject));
        
        if (gameObject.Children.Contains(child))
        {
            throw new InvalidOperationException("Child already exists.");
        }

        gameObject.Children.Add(child);
    }
    
    /// <summary>
    /// Adds a child to a game object.
    /// </summary>
    /// <param name="scene">A scene.</param>
    /// <param name="skybox">A skybox to be added to a scene.</param>
    /// <exception cref="InvalidOperationException">Thrown, when such child already exists in game object children.</exception>
    public static void AddSkybox(this Scene scene, Skybox skybox)
    {
        if (skybox == null) throw new ArgumentNullException(nameof(skybox));
        skybox.Parent = scene ?? throw new ArgumentNullException(nameof(scene));
        scene.Skybox = skybox;
    }
}