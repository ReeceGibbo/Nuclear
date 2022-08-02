using Assimp;
using NuclearGame.Components;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Camera = NuclearGame.Components.Camera;
using Material = NuclearGame.Components.Material;
using Mesh = NuclearGame.Components.Mesh;
using PrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType;

namespace NuclearGame.Rendering.OpenGl;

public class OpenGlRenderPipeline : RenderPipeline
{
    private readonly List<GlRenderableObject> GlRenderObjects;
    private readonly GlShader Shader;

    private Camera comCamera;
    private GlCamera Camera;
    private Transform CameraTransform;
    
    public OpenGlRenderPipeline(int width, int height) : base(width, height)
    {
        GlRenderObjects = new List<GlRenderableObject>();
        Shader = new GlShader("Shaders/basic.vert", "Shaders/basic.frag");
    }

    public override int GenerateMesh(Mesh mesh, Material material)
    {
        // Get Mesh Path and load model
        var glMesh = new GlMesh(mesh.GetMeshData());
        var glMaterial = new GlMaterial(glMesh.Vao, material, Shader);
        
        GlRenderObjects.Add(new GlRenderableObject
        {
            Mesh = glMesh,
            Material = glMaterial
        });

        return GlRenderObjects.Count - 1;
    }

    public override void RenderMesh(Transform transform, int pointer)
    {
        var renderObject = GlRenderObjects[pointer];
        DrawMesh(transform, renderObject);
    }
    
    public override void StartRender()
    {
        GL.Viewport(0, 0, Width, Height);
        GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Back);
        GL.Enable(EnableCap.Multisample);
        GL.Hint(HintTarget.MultisampleFilterHintNv, HintMode.Nicest);
        
        Shader.Use();
        Shader.SetMatrix4("view", Camera.View);
        Shader.SetMatrix4("projection", Camera.Projection);
        Shader.SetVector3("lightPos", new Vector3(-50, -4, -10));
        Shader.SetVector3("cameraPos", CameraTransform.Position);
    }

    public override void StopRender()
    {
        
    }

    private void DrawMesh(Transform transform, GlRenderableObject renderObject)
    {
        GL.BindVertexArray(renderObject.Mesh.Vao);

        var model = Matrix4.CreateTranslation(transform.Position.X, transform.Position.Y, transform.Position.Z);

        Shader.Use();
        Shader.SetMatrix4("model", model);

        renderObject.Material.Diffuse.Use(TextureUnit.Texture0);

        GL.DrawElements(PrimitiveType.Triangles, renderObject.Mesh.IndicesCount, DrawElementsType.UnsignedInt, 0);
    }

    public override void SetMainCamera(Transform transform, Camera camera)
    {
        comCamera = camera;
        Camera = new GlCamera(transform, camera, Width, Height);
        CameraTransform = transform;
    }

    public override void UpdateMainCameraTransform(Transform transform)
    {
        Camera.UpdateView(transform);
        CameraTransform = transform;
    }
    
    public override void Resize(int width, int height)
    {
        Width = width;
        Height = height;

        Camera.Resize(Width, Height, comCamera);
    }
    
}