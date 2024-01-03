using Common;

namespace LightsTestTk.Models;


public interface IMaterial
{
    IShader Shader { get; }
    
    void Use();
}