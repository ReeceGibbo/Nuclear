#version 450
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aNormals;

layout(set = 0, binding = 0) uniform ModelBuffer {
    mat4 model;
};
layout(set = 0, binding = 1) uniform ViewBuffer {
    mat4 view;
};
layout(set = 0, binding = 2) uniform ProjectionBuffer {
    mat4 projection;
};

layout (location = 0) out vec2 texCoord;

void main()
{
    texCoord = aTexCoord;

    gl_Position = projection * view * model * vec4(aPosition, 1.0);
}