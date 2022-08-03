using System.Numerics;
using System.Text;
using Assimp;
using NuclearGame.Components;
using NuclearGame.Components.Data;
using NuclearGame.Rendering;
using NuclearGame.Utils;
using SharpDX.DXGI;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.SPIRV;
using Camera = NuclearGame.Components.Camera;
using Matrix4x4 = System.Numerics.Matrix4x4;
using Mesh = NuclearGame.Components.Mesh;

namespace NuclearGame;

public class Game
{

    private readonly Sdl2Window _window;
    private readonly GraphicsDevice _graphicsDevice;
    
    private CommandList _commandList;

    private Transform _cameraTransform;
    private Camera _camera;
    private DeviceBuffer _projectionBuffer;
    private DeviceBuffer _viewBuffer;

    private Transform _meshTransform;
    private Mesh _mesh;
    private DeviceBuffer _modelBuffer;

    private Pipeline _pipeline;
    private ResourceSet _vertexResourceSet;
    private ResourceSet _fragmentResourceSet;

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

        _projectionBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer | BufferUsage.Dynamic));
        _viewBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer | BufferUsage.Dynamic));
        _modelBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer | BufferUsage.Dynamic));

        var vertexLayout = new VertexLayoutDescription(
            new VertexElementDescription("aPosition", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3),
            new VertexElementDescription("aTexCoord", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
            new VertexElementDescription("aNormals", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3)
        );

        var vertexResources = factory.CreateResourceLayout(new ResourceLayoutDescription(
            new ResourceLayoutElementDescription("ModelBuffer", ResourceKind.UniformBuffer, ShaderStages.Vertex),
            new ResourceLayoutElementDescription("ViewBuffer", ResourceKind.UniformBuffer, ShaderStages.Vertex),
            new ResourceLayoutElementDescription("ProjectionBuffer", ResourceKind.UniformBuffer, ShaderStages.Vertex)
        ));

        var fragmentResources = factory.CreateResourceLayout(new ResourceLayoutDescription(
            new ResourceLayoutElementDescription("diffuseTexture", ResourceKind.TextureReadWrite, ShaderStages.Fragment),
            new ResourceLayoutElementDescription("diffuseSampler", ResourceKind.Sampler, ShaderStages.Fragment)
        ));

        var shaderSet = new ShaderSetDescription(
            new [] { vertexLayout }, 
            factory.CreateFromSpirv(
                new ShaderDescription(ShaderStages.Vertex, Encoding.UTF8.GetBytes(File.ReadAllText(Assets.GetPath("Shaders/basic.vert"))), "main"), 
                new ShaderDescription(ShaderStages.Fragment, Encoding.UTF8.GetBytes(File.ReadAllText(Assets.GetPath("Shaders/basic.frag"))), "main")
                )
        );

        _pipeline = factory.CreateGraphicsPipeline(new GraphicsPipelineDescription(
            BlendStateDescription.SingleOverrideBlend,
            DepthStencilStateDescription.DepthOnlyLessEqual,
            new RasterizerStateDescription(FaceCullMode.Back, PolygonFillMode.Solid, FrontFace.CounterClockwise, true, false),
            PrimitiveTopology.TriangleList,
            shaderSet,
            new [] { vertexResources },
            _graphicsDevice.SwapchainFramebuffer.OutputDescription
        ));

        _vertexResourceSet =
            factory.CreateResourceSet(new ResourceSetDescription(vertexResources, _modelBuffer, _viewBuffer,
                _projectionBuffer));
        
        //_fragmentResourceSet = factory.CreateResourceSet(new ResourceSetDescription(fragmentResources, ))

        CreateEntities();
    }

    private void CreateEntities()
    {
        _cameraTransform = new Transform
        {
            Position = new Vector3(0, 0, 0),
            EulerAngles = new Vector3(),
            Scale = new Vector3()
        };
        _camera = new Camera
        {
            FieldOfView = 70f,
            Near = 0.01f,
            Far = 1000f
        };
        _camera.InitCamera(_window.Width, _window.Height, ref _cameraTransform);

        var assimp = new AssimpContext();
        var scene = assimp.ImportFile(Assets.GetPath("Models/constructor-sketchfab.obj"), PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs | PostProcessSteps.OptimizeMeshes);
        var sceneMesh = scene.Meshes[0];
        
        var vertices = sceneMesh.Vertices.Select(vector => new Vector3(vector.X, vector.Y, vector.Z)).ToArray();
        var normals = sceneMesh.Normals.Select(vector => new Vector3(vector.X, vector.Y, vector.Z)).ToArray();
        var uvs = sceneMesh.TextureCoordinateChannels[0].Select(vector => new Vector2(vector.X, vector.Y)).ToArray();
        
        var meshData = new MeshData
        {
            Vertices = vertices,
            Normals = normals,
            Uvs = uvs,
            Indices = sceneMesh.GetUnsignedIndices()
        };
        
        _meshTransform = new Transform
        {
            Position = new Vector3(0, -3, -15),
            EulerAngles = new Vector3(),
            Scale = new Vector3()
        };
        
        _mesh = new Mesh();
        _mesh.LoadMesh(_graphicsDevice, meshData);
    }

    public void Render()
    {
        _commandList.Begin();
        
        _commandList.UpdateBuffer(_modelBuffer, 0, Matrix4x4.CreateTranslation(_meshTransform.Position));
        _commandList.UpdateBuffer(_viewBuffer, 0, _camera.GetViewMatrix());
        _commandList.UpdateBuffer(_projectionBuffer, 0, _camera.GetProjectionMatrix());

        _commandList.SetFramebuffer(_graphicsDevice.SwapchainFramebuffer);
        _commandList.SetViewport(0, new Viewport(0, 0, _graphicsDevice.SwapchainFramebuffer.Width, _graphicsDevice.SwapchainFramebuffer.Height, 0, 1));
        _commandList.ClearColorTarget(0, RgbaFloat.LightGrey);
        _commandList.ClearDepthStencil(1f);
        
        _commandList.SetPipeline(_pipeline);
        
        _commandList.SetVertexBuffer(0, _mesh.GetVertexBuffer());
        _commandList.SetIndexBuffer(_mesh.GetIndexBuffer(), IndexFormat.UInt32);
        
        _commandList.SetGraphicsResourceSet(0, _vertexResourceSet);
        //_commandList.SetGraphicsResourceSet(1, null);

        _commandList.DrawIndexed(_mesh.GetIndexCount());
        
        _commandList.End();

        _graphicsDevice.SubmitCommands(_commandList);
        _graphicsDevice.SwapBuffers();
        _graphicsDevice.WaitForIdle();
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