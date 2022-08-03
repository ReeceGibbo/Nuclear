using System.Numerics;

namespace NuclearGame.Components.Data;

public struct MeshVertexBufferData
{
    public const uint SizeInBytes = 32;
    
    public float PosX;
    public float PosY;
    public float PosZ;

    public float TexU;
    public float TexV;

    public float NormalX;
    public float NormalY;
    public float NormalZ;

    public MeshVertexBufferData(Vector3 pos, Vector2 uv, Vector3 normal)
    {
        PosX = pos.X;
        PosY = pos.Y;
        PosZ = pos.Z;

        TexU = uv.X;
        TexV = uv.Y;
        
        NormalX = normal.X;
        NormalY = normal.Y;
        NormalZ = normal.Z;
    }
}