namespace LightsTestTk.Models.Shaders;

using OpenTK.Graphics.OpenGL4;

using Common;

using LightsTestTk.Models.Lights;

public class CubeShader : IShader
{
    private readonly Shader _shader;

    public string Name => "cube";
    
    
    public CubeShader()
    {
        _shader = new Shader(
            File.ReadAllText("Resources/Shaders/shader.vert"),
            File.ReadAllText("Resources/Shaders/lighting.frag"));
    }

    
    public int GetAttributeLocation(string name)
    {
        return _shader.GetAttribLocation(name);
    }
    
    
    public void Use(Scene scene, IGameObject gameObject)
    {
        var camera = scene.Camera;
        var material = gameObject.Material;
        
        material.DiffuseMap.Use(TextureUnit.Texture0);
        material.SpecularMap.Use(TextureUnit.Texture1);
        
        _shader.Use();
        
        _shader.SetInt("material.diffuse", 0);
        _shader.SetInt("material.specular", 1);
        
        _shader.SetVector3("material.specular", material.Specular);
        _shader.SetFloat("material.shininess", material.Shininess);
        
        _shader.SetMatrix4("view", camera.GetViewMatrix());
        _shader.SetMatrix4("projection", camera.GetProjectionMatrix());
        _shader.SetVector3("viewPos", camera.Position);
        _shader.SetMatrix4("model", gameObject.ModelMatrix);
        
        /*
           Here we set all the uniforms for the 5/6 types of lights we have. We have to set them manually and index
           the proper PointLight struct in the array to set each uniform variable. This can be done more code-friendly
           by defining light types as classes and set their values in there, or by using a more efficient uniform approach
           by using 'Uniform buffer objects', but that is something we'll discuss in the 'Advanced GLSL' tutorial.
        */
        
        // Directional light
        UpdateDirectionalLightUniforms(scene.DirectionalLight);

        // Point lights
        foreach (var pointLight in scene.PointLights)
        {
            UpdatePointLightUniforms(pointLight);
        }

        // Spot light
        scene.SpotLight.Position = camera.Position;
        scene.SpotLight.Direction = camera.Front;
        UpdateSpotLightUniforms(scene.SpotLight);
    }
    
    
    private void UpdateSpotLightUniforms(SpotLight spotLight)
    {
        _shader.SetVector3(spotLight.PositionUniformName, spotLight.Position);
        _shader.SetVector3(spotLight.DirectionUniformName, spotLight.Direction);
        _shader.SetVector3(spotLight.AmbientUniformName, spotLight.Ambient);
        _shader.SetVector3(spotLight.DiffuseUniformName, spotLight.Diffuse);
        _shader.SetVector3(spotLight.SpecularUniformName, spotLight.Specular);
        _shader.SetFloat(spotLight.ConstantUniformName, spotLight.Constant);
        _shader.SetFloat(spotLight.LinearUniformName, spotLight.Linear);
        _shader.SetFloat(spotLight.QuadraticUniformName, spotLight.Quadratic);
        _shader.SetFloat(spotLight.CutOffUniformName, spotLight.CutOff);
        _shader.SetFloat(spotLight.OuterCutOffUniformName, spotLight.OuterCutOff);
    }

    private void UpdateDirectionalLightUniforms(DirectionalLight directionalLight)
    {
        _shader.SetVector3(directionalLight.DirectionUniformName, directionalLight.Direction);
        _shader.SetVector3(directionalLight.AmbientUniformName, directionalLight.Ambient);
        _shader.SetVector3(directionalLight.DiffuseUniformName, directionalLight.Diffuse);
        _shader.SetVector3(directionalLight.SpecularUniformName, directionalLight.Specular);
    }

    private void UpdatePointLightUniforms(PointLight pointLight)
    {
        _shader.SetVector3(pointLight.PositionUniformName, pointLight.Position);
        _shader.SetVector3(pointLight.AmbientUniformName, pointLight.Ambient);
        _shader.SetVector3(pointLight.DiffuseUniformName, pointLight.Diffuse);
        _shader.SetVector3(pointLight.SpecularUniformName, pointLight.Specular);
        _shader.SetFloat(pointLight.ConstantUniformName, pointLight.Constant);
        _shader.SetFloat(pointLight.LinearUniformName, pointLight.Linear);
        _shader.SetFloat(pointLight.QuadraticUniformName, pointLight.Quadratic);
    }
}