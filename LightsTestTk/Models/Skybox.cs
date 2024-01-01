namespace LightsTestTk.Models;

public class Skybox : IGameObject
{
    /// <summary>
    /// The index of the light in the shader.
    /// </summary>
    public int Id { get; }


    public float[] Vertices { get; } =
    {
        // Each side has 2 triangles, each triangle has 3 vertices.
        
        // Positions          Texture coords
        -0.5f, -0.5f, -0.5f,  0.25f, 1f / 3f,  // Front face.
         0.5f, -0.5f, -0.5f,  0.5f,  1f / 3f,
         0.5f,  0.5f, -0.5f,  0.5f,  2f / 3f,
         0.5f,  0.5f, -0.5f,  0.5f,  2f / 3f,
        -0.5f,  0.5f, -0.5f,  0.25f, 2f / 3f,
        -0.5f, -0.5f, -0.5f,  0.25f, 1f / 3f,
    
        -0.5f, -0.5f,  0.5f,  1.0f,  1f / 3f,  // Back face.
         0.5f, -0.5f,  0.5f,  0.75f, 1f / 3f,
         0.5f,  0.5f,  0.5f,  0.75f, 2f / 3f,
         0.5f,  0.5f,  0.5f,  0.75f, 2f / 3f,
        -0.5f,  0.5f,  0.5f,  1.0f,  2f / 3f,
        -0.5f, -0.5f,  0.5f,  1.0f,  1f / 3f,
    
        -0.5f,  0.5f,  0.5f,  0.0f,  2f / 3f,  // Left face.
        -0.5f,  0.5f, -0.5f,  0.25f, 2f / 3f,
        -0.5f, -0.5f, -0.5f,  0.25f, 1f / 3f,
        -0.5f, -0.5f, -0.5f,  0.25f, 1f / 3f,
        -0.5f, -0.5f,  0.5f,  0.0f,  1f / 3f,
        -0.5f,  0.5f,  0.5f,  0.0f,  2f / 3f,
    
         0.5f,  0.5f,  0.5f,  0.75f, 2f / 3f,  // Right face.
         0.5f,  0.5f, -0.5f,  0.5f,  2f / 3f,
         0.5f, -0.5f, -0.5f,  0.5f,  1f / 3f,
         0.5f, -0.5f, -0.5f,  0.5f,  1f / 3f,
         0.5f, -0.5f,  0.5f,  0.75f, 1f / 3f,
         0.5f,  0.5f,  0.5f,  0.75f, 2f / 3f,
    
        -0.5f, -0.5f, -0.5f,  0.25f, 1f / 3f,  // Bottom face.
         0.5f, -0.5f, -0.5f,  0.5f,  1f / 3f,
         0.5f, -0.5f,  0.5f,  0.5f,  0.0f,
         0.5f, -0.5f,  0.5f,  0.5f,  0.0f,
        -0.5f, -0.5f,  0.5f,  0.25f, 0.0f,
        -0.5f, -0.5f, -0.5f,  0.25f, 1f / 3f,
    
        -0.5f,  0.5f, -0.5f,  0.25f, 2f / 3f,  // Top face.
         0.5f,  0.5f, -0.5f,  0.5f,  2f / 3f,
         0.5f,  0.5f,  0.5f,  0.5f,  1.0f,
         0.5f,  0.5f,  0.5f,  0.5f,  1.0f,
        -0.5f,  0.5f,  0.5f,  0.25f, 1.0f,
        -0.5f,  0.5f, -0.5f,  0.25f, 2f / 3f
    };
    
    /// <summary>
    /// Indices count = number of triangles * number of vertices per triangle.
    /// </summary>
    public int IndicesCount { get; }
    
    public int VertexBufferObject { get; set; }
    public int VertexArrayObject { get; set; }

    
    public Skybox(int id)
    {
        Id = id;
        
        // 36 = 6 sides * 2 triangles per side * 3 vertices per triangle.
        IndicesCount = 36;
        
        VertexBufferObject = -1;
        VertexArrayObject = -1;
    }
}