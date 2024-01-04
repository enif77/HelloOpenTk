namespace LightsTestTk.Models;

using Common;

public interface IMaterial
{
    IShader Shader { get; }
    
    void Use();
}