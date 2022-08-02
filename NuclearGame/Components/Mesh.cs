using Assimp;
using NuclearGame.Components.Data;
using OpenTK.Mathematics;

namespace NuclearGame.Components;

public struct Mesh
{
    public string Path;
    private MeshData MeshData;

    public void LoadMesh(AssimpContext assimpContext)
    {
        MeshData = new MeshData();
        var scene = assimpContext.ImportFile(Path, PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs | PostProcessSteps.OptimizeMeshes);
        var sceneMesh = scene.Meshes[0];
        
        var vertices = sceneMesh.Vertices.Select(vector => new Vector3(vector.X, vector.Y, vector.Z)).ToArray();
        var normals = sceneMesh.Normals.Select(vector => new Vector3(vector.X, vector.Y, vector.Z)).ToArray();
        var uvs = sceneMesh.TextureCoordinateChannels[0].Select(vector => new Vector2(vector.X, vector.Y)).ToArray();

        MeshData.Vertices = vertices;
        MeshData.Normals = normals;
        MeshData.Uvs = uvs;
        MeshData.Indices = sceneMesh.GetUnsignedIndices();
    }

    public MeshData GetMeshData()
    {
        return MeshData;
    }
}