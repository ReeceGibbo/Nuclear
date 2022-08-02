using Assimp;
using NuclearGame.Components;
using NuclearGame.Rendering;
using NuclearGame.Rendering.OpenGl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Camera = NuclearGame.Components.Camera;
using Material = NuclearGame.Components.Material;
using Mesh = NuclearGame.Components.Mesh;

namespace NuclearGame;

public class Game
{

    private readonly GameWindow _window;

    private RenderPipeline RenderPipeline;

    private Transform cameraTransform;
    private Camera camera;

    private Transform modelTransform;
    private int modelIndex;

    private Transform modelTransform2;
    private int modelIndex2;

    public Game(GameWindow gameWindow)
    {
        _window = gameWindow;
    }

    public void Load()
    {
        var normalShader = new GlShader("shaders/basic.vert", "shaders/basic.frag");
        normalShader.SetInt("diffuseTexture", 0);
        
        RenderPipeline = new OpenGlRenderPipeline(_window.Size.X, _window.Size.Y);

        cameraTransform = new Transform
        {
            Position = new Vector3(0, 0, 0),
            EulerAngles = new Vector3(0, 0, 0),
            Scale = new Vector3(1, 1, 1)
        };
        
        camera = new Camera
        {
            FieldOfView = 65f
        };
        RenderPipeline.SetMainCamera(cameraTransform, camera);
        
        var assimp = new AssimpContext();
        LoadModel1(assimp);
        LoadModel2(assimp);
    }

    private void LoadModel1(AssimpContext assimp)
    {
        modelTransform = new Transform
        {
            Position = new Vector3(0, -4, -15),
            EulerAngles = new Vector3(0, 0, 0),
            Scale = new Vector3(1f, 1f, 1f)
        };
        
        var mesh = new Mesh
        {
            Path = "Models/constructor-sketchfab.obj"
        };
        mesh.LoadMesh(assimp);

        var material = new Material
        {
            shader = "basic",
            diffuseTexture = "Models/textures/constructor-sketchfab.png"
        };
        material.LoadTexture();

        modelIndex = RenderPipeline.GenerateMesh(mesh, material);
    }
    
    private void LoadModel2(AssimpContext assimp)
    {
        modelTransform2 = new Transform
        {
            Position = new Vector3(-10, -4, -15),
            EulerAngles = new Vector3(0, 0, 0),
            Scale = new Vector3(1f, 1f, 1f)
        };
        
        var mesh = new Mesh
        {
            Path = "Models/DonutThing/donut.obj"
        };
        mesh.LoadMesh(assimp);

        var material = new Material
        {
            shader = "basic",
            diffuseTexture = "Models/DonutThing/wood.png"
        };
        material.LoadTexture();

        modelIndex2 = RenderPipeline.GenerateMesh(mesh, material);
    }
    
    public void Render(FrameEventArgs e)
    {
        RenderPipeline.StartRender();
        RenderPipeline.RenderMesh(modelTransform, modelIndex);
        RenderPipeline.RenderMesh(modelTransform2, modelIndex2);
        RenderPipeline.StopRender();
    }

    public void Update(FrameEventArgs e, KeyboardState keyboardState, MouseState mouseState)
    {
        var speed = 0.1f;
        var position = cameraTransform.Position;

        var movement = new Vector3(0, 0, 0);
                
        if (keyboardState.IsKeyDown(Keys.W))
        {
            movement.Z = 1;
        }
        if (keyboardState.IsKeyDown(Keys.S))
        {
            movement.Z = -1;
        }
        if (keyboardState.IsKeyDown(Keys.A))
        {
            movement.X = 1;
        }
        if (keyboardState.IsKeyDown(Keys.D))
        {
            movement.X = -1;
        }
        
        var upDown = 0f;
        if (keyboardState.IsKeyDown(Keys.Space))
        {
            upDown -= speed;
        }

        if (keyboardState.IsKeyDown(Keys.LeftShift))
        {
            upDown += speed;
        }

        cameraTransform.Position = position + (movement * speed) + new Vector3(0, upDown, 0);;
        
        // Rotation Movement
        var eulerAngles = cameraTransform.EulerAngles;
        eulerAngles.X += 0.01f * (mouseState.Y - mouseState.PreviousY);
        eulerAngles.Y += 0.01f * (mouseState.X - mouseState.PreviousX);
        eulerAngles.Z = 0f;

        cameraTransform.EulerAngles = eulerAngles;
        
        RenderPipeline.UpdateMainCameraTransform(cameraTransform);
    }

    public void Resize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, e.Width, e.Height);
        RenderPipeline.Resize(e.Width, e.Height);
    }

    public void EditorResize(int width, int height)
    {
        RenderPipeline.Resize(width, height);
    }

    public void Destroy()
    {
        // Unbind all the resources by binding the targets to 0/null.
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);
    }
}