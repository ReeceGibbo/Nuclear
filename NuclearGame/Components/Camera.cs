using System.Numerics;
using NuclearGame.Utils;

namespace NuclearGame.Components;

public struct Camera
{
    public float FieldOfView;
    public float Near;
    public float Far;

    private bool _init;
    private float _aspectRatio;
    private Matrix4x4 _viewMatrix;
    private Matrix4x4 _projectionMatrix;
    
    private void InitCamera(int width, int height, ref Transform transform)
    {
        UpdateProjectionMatrix(width, height);
        UpdateViewMatrix(ref transform);
        _init = true;
    }

    public void UpdateViewMatrix(ref Transform transform)
    {
        var lookRotation = Quaternion.CreateFromYawPitchRoll(0f, 0f, 0f);
        var lookDir = Vector3.Transform(-Vector3.UnitZ, lookRotation);

        _viewMatrix = Matrix4x4.CreateLookAt(transform.Position, transform.Position + lookDir, Vector3.UnitY);
    }
    
    public void UpdateProjectionMatrix(int width, int height)
    {
        _aspectRatio = (float)width / (float)height;

        _projectionMatrix = Matrix4.CreatePerspective(Util.DegreesToRadians(FieldOfView), _aspectRatio, Near, Far);
    }

    public bool HasInit()
    {
        return _init;
    }

    public Matrix4x4 GetViewMatrix()
    {
        return _viewMatrix;
    }

    public Matrix4x4 GetProjectionMatrix()
    {
        return _projectionMatrix;
    }
}