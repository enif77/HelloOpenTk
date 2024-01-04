namespace Common;

using OpenTK.Mathematics;

/// <summary>
/// A shader, that does nothing.
/// </summary>
public class NullShader : IShader
{
    public void Use()
    {
    }

    public int GetAttribLocation(string attribName) => -1;

    public void SetInt(string name, int data)
    {
    }

    public void SetFloat(string name, float data)
    {
    }

    public void SetMatrix4(string name, Matrix4 data)
    {
    }

    public void SetVector3(string name, Vector3 data)
    {
    }
}