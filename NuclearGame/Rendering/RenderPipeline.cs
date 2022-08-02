using NuclearGame.Components;

namespace NuclearGame.Rendering;

public abstract class RenderPipeline
{
    protected int Width, Height;

    protected RenderPipeline(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public abstract int GenerateMesh(Mesh mesh, Material material);

    public abstract void StartRender();
    
    public abstract void StopRender();
    public abstract void RenderMesh(Transform transform, int pointer);

    public abstract void SetMainCamera(Transform transform, Camera camera);

    public abstract void UpdateMainCameraTransform(Transform transform);

    public abstract void Resize(int width, int height);
}