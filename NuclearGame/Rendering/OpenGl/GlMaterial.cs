using NuclearGame.Components;
using OpenTK.Graphics.OpenGL4;

namespace NuclearGame.Rendering.OpenGl;

public class GlMaterial
{
    public GlTexture Diffuse;
    
    public int VertexLocation { get; set; }
    public int TexCoordLocation { get; set; }
    public int NormalLocation { get; set; }

    public GlMaterial(int vao, Material material, GlShader shader)
    {
        Diffuse = new GlTexture(material.GetTextureData());
        
        GL.BindVertexArray(vao);

        // Tell the GPU where the vertices data is
        VertexLocation = shader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(VertexLocation);
        GL.VertexAttribPointer(VertexLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
                    
        // Texture Coords
        TexCoordLocation = shader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(TexCoordLocation);
        GL.VertexAttribPointer(TexCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
                    
        // Normals
        NormalLocation = shader.GetAttribLocation("aNormals");
        GL.EnableVertexAttribArray(NormalLocation);
        GL.VertexAttribPointer(NormalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
    }
}