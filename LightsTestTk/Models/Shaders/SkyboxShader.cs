namespace LightsTestTk.Models.Shaders;

using OpenTK.Graphics.OpenGL4;

using Common;

public class SkyboxShader : IShader
{
    private readonly Shader _shader;

    public string Name => "skybox";
    
    
    public SkyboxShader()
    {
        _shader = new Shader(
            File.ReadAllText("Resources/Shaders/skybox.vert"),
            File.ReadAllText("Resources/Shaders/skybox.frag"));
    }

    
    public int GetAttributeLocation(string name)
    {
        return _shader.GetAttribLocation(name);
    }
    
    
    public void Use(Scene scene, IGameObject gameObject)
    {
        var camera = scene.Camera;
        
        gameObject.Material.DiffuseMap.Use(TextureUnit.Texture0);
        
        _shader.Use();
        
        _shader.SetInt("texture0", 0);
        
        _shader.SetMatrix4("view", camera.GetViewMatrix());
        _shader.SetMatrix4("projection", camera.GetProjectionMatrix());
        _shader.SetMatrix4("model", gameObject.ModelMatrix);
    }
}