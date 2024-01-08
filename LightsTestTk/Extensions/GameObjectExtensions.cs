namespace LightsTestTk.Extensions;

using OpenTK.Graphics.OpenGL4;

using LightsTestTk.Models;

/// <summary>
/// Game object related extensions.
/// </summary>
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
        ArgumentNullException.ThrowIfNull(child);
        if (gameObject.Children.Contains(child))
        {
            throw new InvalidOperationException("Child already exists in the parent object.");
        }
        
        child.Parent = gameObject;
        gameObject.Children.Add(child);
    }
    
    /// <summary>
    /// Generates a VBO for a game object.
    /// </summary>
    /// <param name="gameObject">A game object instance.</param>
    public static void GenerateVertexObjectBuffer(this IGameObject gameObject)
    {
        gameObject.VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, gameObject.VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, gameObject.Vertices.Length * sizeof(float), gameObject.Vertices, BufferUsageHint.StaticDraw);
    }
    
    /// <summary>
    /// Generates a VAO for a game object with position-texture VBO format.
    /// </summary>
    /// <param name="gameObject">A game object instance.</param>
    /// <exception cref="InvalidOperationException">When vertex object buffer is not initialized yet.</exception>
    /// <exception cref="InvalidOperationException">When vertex array object is already initialized.</exception>
    public static void GenerateVertexArrayObjectForPosTexVbo(this IGameObject gameObject)
    {
        if (gameObject.VertexBufferObject <= 0)
        {
            throw new InvalidOperationException("Vertex buffer object is not initialized.");
        }
        
        if (gameObject.VertexArrayObject > 0)
        {
            throw new InvalidOperationException("Vertex array object is already initialized.");
        }
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, gameObject.VertexBufferObject);

        gameObject.VertexArrayObject = GL.GenVertexArray();
        
        GL.BindVertexArray(gameObject.VertexArrayObject);

        var shader = gameObject.Material.Shader;
        
        var positionLocation = shader.GetAttributeLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            
        var texCoordLocation = shader.GetAttributeLocation("aTexCoords");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
    }
    
    /// <summary>
    /// Generates a VAO for a game object with position-normal-texture VBO format.
    /// </summary>
    /// <param name="gameObject">A game object instance.</param>
    /// <exception cref="InvalidOperationException">When vertex object buffer is not initialized yet.</exception>
    /// <exception cref="InvalidOperationException">When vertex array object is already initialized.</exception>
    public static void GenerateVertexArrayObjectForPosNormTexVbo(this IGameObject gameObject)
    {
        if (gameObject.VertexBufferObject <= 0)
        {
            throw new InvalidOperationException("Vertex buffer object is not initialized.");
        }
        
        if (gameObject.VertexArrayObject > 0)
        {
            throw new InvalidOperationException("Vertex array object is already initialized.");
        }
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, gameObject.VertexBufferObject);

        gameObject.VertexArrayObject = GL.GenVertexArray();
        
        GL.BindVertexArray(gameObject.VertexArrayObject);

        var shader = gameObject.Material.Shader;
        
        var positionLocation = shader.GetAttributeLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

        var normalLocation = shader.GetAttributeLocation("aNormal");
        GL.EnableVertexAttribArray(normalLocation);
        GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

        var texCoordLocation = shader.GetAttributeLocation("aTexCoords");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
    }
}