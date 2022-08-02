using OpenTK.Mathematics;

namespace NuclearGame.Components.Data;

public struct MeshData
{
    public Vector3[] Vertices { get; set; }
    public Vector3[] Normals { get; set; }
    public Vector2[] Uvs { get; set; }
    public uint[] Indices { get; set; }
}