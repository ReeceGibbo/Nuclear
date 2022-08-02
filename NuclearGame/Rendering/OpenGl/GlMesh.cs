using NuclearGame.Components;
using NuclearGame.Components.Data;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace NuclearGame.Rendering.OpenGl;

public class GlMesh
{
    private MeshData MeshData;
    
    // OpenGL
    public int Vao { get; }
    public int Vbo { get; }
    public int Ebo { get; }
    
    public int IndicesCount { get; }

    public GlMesh(MeshData meshData)
    {
        MeshData = meshData;
        IndicesCount = meshData.Indices.Length;
        
        Vao = GL.GenVertexArray();
        GL.BindVertexArray(Vao);

        Vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
        
        var bufferData = new float[meshData.Vertices.Length * 8];

        for (var index = 0; index < meshData.Vertices.Length; index++)
        {
            var vertex = meshData.Vertices[index];
            var normal = meshData.Normals[index];
            var uv = meshData.Uvs[index];

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
        
        GL.BufferData(BufferTarget.ArrayBuffer, bufferData.Length * sizeof(float), bufferData, BufferUsageHint.StaticDraw);
            
        // Indices Buffer
        Ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, meshData.Indices.Length * sizeof(uint), meshData.Indices, BufferUsageHint.StaticDraw);
    }
}