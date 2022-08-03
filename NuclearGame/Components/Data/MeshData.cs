using System.Numerics;

namespace NuclearGame.Components.Data;

public struct MeshData
{
    public Vector3[] Vertices { get; set; }
    public Vector3[] Normals { get; set; }
    public Vector2[] Uvs { get; set; }
    public uint[] Indices { get; set; }
    
    public uint GetVertexBufferSize()
    {
        return (uint) (MeshVertexBufferData.SizeInBytes * Vertices.Length);
    }

    public uint GetIndexBufferSize()
    {
        return (uint) (4 * Indices.Length);
    }

    public MeshVertexBufferData[] GetVertexBufferData()
    {
        var bufferData = new MeshVertexBufferData[Vertices.Length];

        for (var index = 0; index < Vertices.Length; index++)
        {
            var vertex = Vertices[index];
            var normal = Normals[index];
            var uv = Uvs[index];

            bufferData[index] = new MeshVertexBufferData(vertex, uv, normal);
        }
        
        return bufferData;
    }
}