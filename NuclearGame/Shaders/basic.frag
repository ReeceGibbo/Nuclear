#version 450

layout(location = 0) out vec4 FragColor;

layout(location = 0) in vec2 texCoord;

layout(set = 2, binding = 0) uniform texture2D diffuseTexture;
layout(set = 2, binding = 1) uniform sampler diffuseSampler;

void main()
{
    FragColor = texture(sampler2D(diffuseTexture, diffuseSampler), texCoord);
}