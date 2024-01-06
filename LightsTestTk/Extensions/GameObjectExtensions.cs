using LightsTestTk.Models;
using OpenTK.Graphics.OpenGL4;

namespace LightsTestTk.Extensions;

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


    /// <summary>
    /// Adds a shader to the scene.
    /// </summary>
    /// <param name="scene">A scene.</param>
    /// <param name="shader">An IShader instance.</param>
    /// <exception cref="InvalidOperationException">If a shader with the shaderName exists in the shaders collection.</exception>
    /// <exception cref="ArgumentException">If the shaderName parameter is null, empty or whitespace.</exception>
    /// <exception cref="ArgumentNullException">If the shader is null.</exception>
    public static void AddShader(this Scene scene, IShader shader)
    {
        if (shader == null) throw new ArgumentNullException(nameof(shader));
        if (scene.Shaders.ContainsKey(shader.Name)) throw new InvalidOperationException($"Shader with the '{shader.Name}' name already exists.");
        
        scene.Shaders.Add(shader.Name, shader);
    }
    
    
    public static void GenerateVertexObjectBuffer(this IGameObject gameObject)
    {
        gameObject.VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, gameObject.VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, gameObject.Vertices.Length * sizeof(float), gameObject.Vertices, BufferUsageHint.StaticDraw);
    }
    
    
    public static void GenerateVertexArrayObjectForPosTexVbo(this IGameObject gameObject, IShader shader)
    {
        if (shader == null) throw new ArgumentNullException(nameof(shader));
        
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

        var positionLocation = shader.GetAttributeLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            
        var texCoordLocation = shader.GetAttributeLocation("aTexCoords");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
    }
    
    
    public static void GenerateVertexArrayObjectForPosNormTexVbo(this IGameObject gameObject, IShader shader)
    {
        if (shader == null) throw new ArgumentNullException(nameof(shader));
        
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