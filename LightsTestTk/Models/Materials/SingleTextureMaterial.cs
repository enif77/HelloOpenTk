namespace LightsTestTk.Models.Materials;

using Common;
using OpenTK.Graphics.OpenGL4;

/// <summary>
/// A material that uses a single texture with no lighting. 
/// </summary>
public class SingleTextureMaterial : IMaterial
{
    public Texture Texture { get; }
    public IShader Shader { get; }
    
    
    public SingleTextureMaterial(Texture texture, Shader shader)
    {
        Texture = texture ?? throw new ArgumentNullException(nameof(texture));
        Shader = shader ?? throw new ArgumentNullException(nameof(shader));
    }
    
    
    public void Use()
    {
        Texture.Use(TextureUnit.Texture0);
        Shader.Use();
        
        Shader.SetInt("material.diffuse", 0);
    }
}