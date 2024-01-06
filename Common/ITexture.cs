namespace Common;

using OpenTK.Graphics.OpenGL4;

public interface ITexture
{
    void Use(TextureUnit unit);
}