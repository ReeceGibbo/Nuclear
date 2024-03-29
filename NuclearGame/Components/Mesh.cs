﻿using NuclearGame.Components.Data;
using Veldrid;

namespace NuclearGame.Components;

public struct Mesh
{
    public string Path;
    private string _loadedPath;
    
    private bool _init;
    private uint _indexCount;
    private DeviceBuffer _vertexBuffer;
    private DeviceBuffer _indexBuffer;

    public void LoadMesh(GraphicsDevice device, MeshData meshData)
    {
        var factory = device.ResourceFactory;
        
        _vertexBuffer =
            factory.CreateBuffer(new BufferDescription(meshData.GetVertexBufferSize(), BufferUsage.VertexBuffer));
        _indexBuffer =
            factory.CreateBuffer(new BufferDescription(meshData.GetIndexBufferSize(), BufferUsage.IndexBuffer));

        device.UpdateBuffer(_vertexBuffer, 0, meshData.GetVertexBufferData());
        device.UpdateBuffer(_indexBuffer, 0, meshData.Indices);

        _indexCount = (uint) meshData.Indices.Length;
    }

    private bool HasInit()
    {
        return _init;
    }

    private bool HasPathChanged()
    {
        if (Path != _loadedPath)
        {
            _init = false;
            return true;
        }

        return false;
    }

    public DeviceBuffer GetVertexBuffer()
    {
        return _vertexBuffer;
    }

    public DeviceBuffer GetIndexBuffer()
    {
        return _indexBuffer;
    }

    public uint GetIndexCount()
    {
        return _indexCount;
    }
    
}