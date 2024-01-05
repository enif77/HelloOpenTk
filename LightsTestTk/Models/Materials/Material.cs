namespace LightsTestTk.Models.Materials;

using Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

/// <summary>
/// A material that uses multiple textures with lighting. 
/// </summary>
public class Material : IMaterial
{
    public Texture DiffuseMap { get; }
    public Texture SpecularMap { get; }
    public Vector3 Specular { get; set; }
    public float Shininess { get; set; }
    
    public IShader Shader { get; }
    
    
    public Material(Texture diffuseMap, Texture specularMap, Shader shader)
    {
        DiffuseMap = diffuseMap ?? throw new ArgumentNullException(nameof(diffuseMap));
        SpecularMap = specularMap ?? throw new ArgumentNullException(nameof(specularMap));
        Specular = new Vector3(0.5f, 0.5f, 0.5f);
        Shininess = 32.0f;
        Shader = shader ?? throw new ArgumentNullException(nameof(shader));
    }
    
    
    public void Use()
    {
        DiffuseMap.Use(TextureUnit.Texture0);
        SpecularMap.Use(TextureUnit.Texture1);
        
        Shader.Use();
        
        Shader.SetInt("material.diffuse", 0);
        Shader.SetInt("material.specular", 1);
        Shader.SetVector3("material.specular", Specular);
        Shader.SetFloat("material.shininess", Shininess);
    }
}