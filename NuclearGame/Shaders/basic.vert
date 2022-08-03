#version 450
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aNormals;

layout(set = 1, binding = 0) uniform Model {
    mat4 model;
};
layout(set = 1, binding = 1) uniform View {
    mat4 view;
};
layout(set = 1, binding = 2) uniform Projection {
    mat4 projection;
};

layout (location = 0) out vec2 texCoord;

void main()
{
    texCoord = aTexCoord;

    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
}