using OpenTK.Graphics.OpenGL4;
using TextureWrapMode = OpenTK.Graphics.OpenGL.TextureWrapMode;

namespace NuclearGame;

public class OpenGlFramebuffer
{

    private int rendererId;
    private int colorAttachmentId;
    private int depthAttachmentId;

    private int width;
    private int height;
    
    public OpenGlFramebuffer(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void Generate()
    {
        if (rendererId == 0)
        {
            Dispose();
        }
        
        GL.CreateFramebuffers(1, out rendererId);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, rendererId);
        
        GL.CreateTextures(TextureTarget.Texture2D, 1, out colorAttachmentId);
        GL.BindTexture(TextureTarget.Texture2D, colorAttachmentId);
        
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
        GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, new int[] {(int) TextureMinFilter.Linear} );
        GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, new int[] {(int) TextureMagFilter.Linear} );
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
            TextureTarget.Texture2D, colorAttachmentId, 0);

        
        GL.CreateTextures(TextureTarget.Texture2D, 1, out depthAttachmentId);
        GL.BindTexture(TextureTarget.Texture2D, depthAttachmentId);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Depth24Stencil8, width, height, 0, PixelFormat.DepthStencil, PixelType.UnsignedInt248, IntPtr.Zero);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment,
            TextureTarget.Texture2D, depthAttachmentId, 0);

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public void Resize(int width, int height)
    {
        this.width = width;
        this.height = height;
        Generate();
    }

    public void Dispose()
    {
        GL.DeleteFramebuffers(1, ref rendererId);
        GL.DeleteTextures(1, ref colorAttachmentId);
        GL.DeleteTextures(1, ref depthAttachmentId);
    }

    public void Bind()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, rendererId);
        GL.Viewport(0, 0, width, height);
    }

    public void Unbind()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public IntPtr GetColorAttachment()
    {
        return (IntPtr)colorAttachmentId;
    }

    public int GetRendererId()
    {
        return rendererId;
    }
}