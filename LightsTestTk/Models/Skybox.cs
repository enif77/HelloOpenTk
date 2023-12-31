namespace LightsTestTk.Models;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using LightsTestTk.Extensions;

/// <summary>
/// Skybox.
/// </summary>
public class Skybox : IGameObject, IRenderable
{
    public IGameObject? Parent { get; set; }
    public IList<IGameObject> Children { get; }
    
    public Vector3 Position { get; set; }
    
    /// <summary>
    /// Material used to render the skybox.
    /// </summary>
    public IMaterial Material { get; }
    
    private const float TexCoordsFix = 0.001f;

    public float[] Vertices { get; } =
    {
        // Each side has 2 triangles, each triangle has 3 vertices.
        
        // Positions          Texture coords
        -0.5f, -0.5f, -0.5f,  0.25f + TexCoordsFix, 1f / 3f + TexCoordsFix,  // Front face.
         0.5f, -0.5f, -0.5f,  0.5f  - TexCoordsFix, 1f / 3f + TexCoordsFix,
         0.5f,  0.5f, -0.5f,  0.5f  - TexCoordsFix, 2f / 3f - TexCoordsFix,
         0.5f,  0.5f, -0.5f,  0.5f  - TexCoordsFix, 2f / 3f - TexCoordsFix,
        -0.5f,  0.5f, -0.5f,  0.25f + TexCoordsFix, 2f / 3f - TexCoordsFix,
        -0.5f, -0.5f, -0.5f,  0.25f + TexCoordsFix, 1f / 3f + TexCoordsFix,
    
        -0.5f, -0.5f,  0.5f,  1.0f  - TexCoordsFix, 1f / 3f + TexCoordsFix,  // Back face.
         0.5f, -0.5f,  0.5f,  0.75f + TexCoordsFix, 1f / 3f + TexCoordsFix,
         0.5f,  0.5f,  0.5f,  0.75f + TexCoordsFix, 2f / 3f - TexCoordsFix,
         0.5f,  0.5f,  0.5f,  0.75f + TexCoordsFix, 2f / 3f - TexCoordsFix,
        -0.5f,  0.5f,  0.5f,  1.0f  - TexCoordsFix, 2f / 3f - TexCoordsFix,
        -0.5f, -0.5f,  0.5f,  1.0f  - TexCoordsFix, 1f / 3f + TexCoordsFix,
    
        -0.5f,  0.5f,  0.5f,  0.0f  + TexCoordsFix, 2f / 3f - TexCoordsFix,  // Left face.
        -0.5f,  0.5f, -0.5f,  0.25f - TexCoordsFix, 2f / 3f - TexCoordsFix,
        -0.5f, -0.5f, -0.5f,  0.25f - TexCoordsFix, 1f / 3f + TexCoordsFix,
        -0.5f, -0.5f, -0.5f,  0.25f - TexCoordsFix, 1f / 3f + TexCoordsFix,
        -0.5f, -0.5f,  0.5f,  0.0f  + TexCoordsFix, 1f / 3f + TexCoordsFix,
        -0.5f,  0.5f,  0.5f,  0.0f  + TexCoordsFix, 2f / 3f - TexCoordsFix,
    
         0.5f,  0.5f,  0.5f,  0.75f - TexCoordsFix, 2f / 3f - TexCoordsFix,  // Right face.
         0.5f,  0.5f, -0.5f,  0.5f  + TexCoordsFix, 2f / 3f - TexCoordsFix,
         0.5f, -0.5f, -0.5f,  0.5f  + TexCoordsFix, 1f / 3f + TexCoordsFix,
         0.5f, -0.5f, -0.5f,  0.5f  + TexCoordsFix, 1f / 3f + TexCoordsFix,
         0.5f, -0.5f,  0.5f,  0.75f - TexCoordsFix, 1f / 3f + TexCoordsFix,
         0.5f,  0.5f,  0.5f,  0.75f - TexCoordsFix, 2f / 3f - TexCoordsFix,
    
        -0.5f, -0.5f, -0.5f,  0.25f + TexCoordsFix, 1f / 3f - TexCoordsFix,  // Bottom face.
         0.5f, -0.5f, -0.5f,  0.5f  - TexCoordsFix, 1f / 3f - TexCoordsFix,
         0.5f, -0.5f,  0.5f,  0.5f  - TexCoordsFix, 0.0f + TexCoordsFix,
         0.5f, -0.5f,  0.5f,  0.5f  - TexCoordsFix, 0.0f + TexCoordsFix,
        -0.5f, -0.5f,  0.5f,  0.25f + TexCoordsFix, 0.0f + TexCoordsFix,
        -0.5f, -0.5f, -0.5f,  0.25f + TexCoordsFix, 1f / 3f - TexCoordsFix,
    
        -0.5f,  0.5f, -0.5f,  0.25f + TexCoordsFix, 2f / 3f + TexCoordsFix,  // Top face.
         0.5f,  0.5f, -0.5f,  0.5f  - TexCoordsFix, 2f / 3f + TexCoordsFix,
         0.5f,  0.5f,  0.5f,  0.5f  - TexCoordsFix, 1.0f - TexCoordsFix,
         0.5f,  0.5f,  0.5f,  0.5f  - TexCoordsFix, 1.0f - TexCoordsFix,
        -0.5f,  0.5f,  0.5f,  0.25f + TexCoordsFix, 1.0f - TexCoordsFix,
        -0.5f,  0.5f, -0.5f,  0.25f + TexCoordsFix, 2f / 3f + TexCoordsFix
    };
    
    /// <summary>
    /// Indices count = number of triangles * number of vertices per triangle.
    /// </summary>
    public int IndicesCount { get; }
    
    public int VertexBufferObject { get; set; }
    public int VertexArrayObject { get; set; }
    
    public Matrix4 ModelMatrix { get; set; }

    
    public Skybox(IMaterial material)
    {
        Material = material ?? throw new ArgumentNullException(nameof(material));
        
        Position = new Vector3();
        
        // 36 = 6 sides * 2 triangles per side * 3 vertices per triangle.
        IndicesCount = 36;
        
        VertexBufferObject = -1;
        VertexArrayObject = -1;
        
        ModelMatrix = Matrix4.Identity;
        
        Children = new List<IGameObject>();
    }

    
    private Scene? _scene;

    public void Render()
    {
        _scene ??= this.GetScene();

        // Skybox should be rendered at the camera position.
        ModelMatrix = Matrix4.CreateTranslation(_scene.Camera.Position);
        
        // Skybox should be rendered without depth test.
        GL.Disable(EnableCap.DepthTest);
        
        // Sets shader and its properties.
        Material.Shader.Use(_scene, this);
        
        // Bind skybox data.
        //GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject); // Bound by VAO below.
        GL.BindVertexArray(VertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, IndicesCount);
        
        // Restore depth test.
        GL.Enable(EnableCap.DepthTest);
    }
}