namespace LightsTestTk.Models.Shaders;

public class NullShader : IShader
{
    public string Name => "null";

    
    public int GetAttributeLocation(string name)
    {
        return -1;
    }
    
    
    public void Use(Scene scene, IGameObject gameObject)
    {
        // Do nothing.
    }
}