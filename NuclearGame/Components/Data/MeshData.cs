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
        var verticesSize = 4 * 3 * Vertices.Length;
        var normalsSize = 4 * 3 * Normals.Length;
        var uvsSize = 4 * 2 * Uvs.Length;

        return (uint) (verticesSize + normalsSize + uvsSize);
    }

    public uint GetIndexBufferSize()
    {
        return (uint) (4 * Indices.Length);
    }

    public float[] GetVertexBufferData()
    {
        var bufferData = new float[Vertices.Length * 8];

        for (var index = 0; index < Vertices.Length; index++)
        {
            var vertex = Vertices[index];
            var normal = Normals[index];
            var uv = Uvs[index];

            var pos = (index * 8);

            // Vertices
            bufferData[pos]     = vertex.X;
            bufferData[pos + 1] = vertex.Y;
            bufferData[pos + 2] = vertex.Z;
                
            // UVs
            bufferData[pos + 3] = uv.X;
            bufferData[pos + 4] = uv.Y;
                
            // Normals
            bufferData[pos + 5] = normal.X;
            bufferData[pos + 6] = normal.Y;
            bufferData[pos + 7] = normal.Z;
        }

        return bufferData;
    }
}