using System.Text;
using NuclearGame.Utils;
using SharpDX.DXGI;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.SPIRV;

namespace NuclearGame;

public class Game
{

    private readonly Sdl2Window _window;
    private readonly GraphicsDevice _graphicsDevice;

    private CommandList _commandList;

    public Game(Sdl2Window window, GraphicsDevice graphicsDevice)
    {
        _window = window;
        _graphicsDevice = graphicsDevice;

        Util.UseReverseDepth = graphicsDevice.IsDepthRangeZeroToOne;
        Util.IsClipSpaceYInverted = graphicsDevice.IsClipSpaceYInverted;
    }

    public void Create()
    {
        var factory = _graphicsDevice.ResourceFactory;

        _commandList = factory.CreateCommandList();

        var vertexLayout = new VertexLayoutDescription(
            new VertexElementDescription("aPosition", VertexElementSemantic.Position, VertexElementFormat.Float3), 
            new VertexElementDescription("aTexCoord", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2), 
            new VertexElementDescription("aNormals", VertexElementSemantic.Normal, VertexElementFormat.Float3)
        );

        var vertexResources = factory.CreateResourceLayout(new ResourceLayoutDescription(
            new ResourceLayoutElementDescription("model", ResourceKind.UniformBuffer, ShaderStages.Vertex),
            new ResourceLayoutElementDescription("view", ResourceKind.UniformBuffer, ShaderStages.Vertex),
            new ResourceLayoutElementDescription("projection", ResourceKind.UniformBuffer, ShaderStages.Vertex)
        ));

        var fragmentResources = factory.CreateResourceLayout(new ResourceLayoutDescription(
            new ResourceLayoutElementDescription("diffuseTexture", ResourceKind.TextureReadWrite, ShaderStages.Fragment),
            new ResourceLayoutElementDescription("diffuseSampler", ResourceKind.Sampler, ShaderStages.Fragment)
        ));

        var shaderSet = new ShaderSetDescription(
            new [] { vertexLayout }, 
            factory.CreateFromSpirv(
                new ShaderDescription(ShaderStages.Vertex, Encoding.UTF8.GetBytes(File.ReadAllText(Assets.GetPath("basic.vert"))), "main"), 
                new ShaderDescription(ShaderStages.Fragment, Encoding.UTF8.GetBytes(File.ReadAllText(Assets.GetPath("basic.frag"))), "main")
                )
        );

        var pipeline = factory.CreateGraphicsPipeline(new GraphicsPipelineDescription(
            BlendStateDescription.SingleOverrideBlend,
            DepthStencilStateDescription.DepthOnlyLessEqual,
            RasterizerStateDescription.Default,
            PrimitiveTopology.TriangleList,
            shaderSet,
            new [] { vertexResources, fragmentResources },
            _graphicsDevice.SwapchainFramebuffer.OutputDescription
        ));
    }

    public void Render()
    {
        _commandList.Begin();
        _commandList.SetFramebuffer(_graphicsDevice.SwapchainFramebuffer);
        _commandList.ClearColorTarget(0, RgbaFloat.Blue);
        _commandList.End();

        _graphicsDevice.SubmitCommands(_commandList);
        _graphicsDevice.SwapBuffers();
    }

    public void Update()
    {
        
    }

    public void Resize()
    {
        var width = (uint)_window.Width;
        var height = (uint)_window.Height;

        _graphicsDevice.ResizeMainWindow(width, height);
    }

    public void EditorResize(uint width, uint height)
    {
        _graphicsDevice.ResizeMainWindow(width, height);
    }

    public void Destroy()
    {
        
    }
}