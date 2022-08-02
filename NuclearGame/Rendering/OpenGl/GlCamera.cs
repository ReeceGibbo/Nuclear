using NuclearGame.Components;
using OpenTK.Mathematics;

namespace NuclearGame.Rendering.OpenGl;

public class GlCamera
{
    public float AspectRatio { get; set; }
    
    public Matrix4 View { get; set; }
    public Matrix4 Projection { get; set; }
    
    public GlCamera(Transform transform, Camera camera, int width, int height)
    {
        AspectRatio = (float)width / (float)height;

        Projection =
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(camera.FieldOfView), AspectRatio, 0.01f, 100.0f);
        View = Matrix4.CreateTranslation(transform.Position.X, transform.Position.Y, transform.Position.Z);
    }

    public void UpdateView(Transform transform)
    {
        View = Matrix4.CreateTranslation(transform.Position.X, transform.Position.Y, transform.Position.Z) *
               Matrix4.CreateFromQuaternion(new Quaternion(transform.EulerAngles.X, transform.EulerAngles.Y, transform.EulerAngles.Z));
    }

    public void Resize(int width, int height, Camera camera)
    {
        AspectRatio = (float)width / (float)height;
        
        Projection =
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(camera.FieldOfView), AspectRatio, 0.01f, 100.0f);
    }
}