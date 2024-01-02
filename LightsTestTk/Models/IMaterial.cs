using Common;

namespace LightsTestTk.Models;


public interface IMaterial
{
    Shader Shader { get; }
    
    void Use();
}