#version 460 core

out vec4 FragColor;

in vec2 texCoord;
in vec3 normal;
in vec3 fragPos;

// Lighting
uniform vec3 lightPos;
uniform vec3 cameraPos;

// Textures
uniform sampler2D diffuseTexture;

void main()
{
    // Lighting Calculations
    vec3 lightColor = vec3(1.0, 1.0, 1.0);
    
    vec3 norm = normalize(normal);
    vec3 lightDir = normalize(lightPos - fragPos);
    vec3 cameraDir = normalize(cameraPos - fragPos);
    vec3 halfwayDir = normalize(lightDir + cameraDir);

    // Diffuse
    float diffuse = max(dot(norm, lightDir), 0.0);

    // Specular
    float specularStrength = 0.5;

    float specAmount = pow(max(dot(normal, halfwayDir), 0.0), 16);
    float specular = specAmount * specularStrength;
    
    // Ambient
    float ambient = 0.02;
    
    // Final Result
    vec4 tex = texture(diffuseTexture, texCoord);
    vec3 result = (ambient + diffuse + specular) * lightColor * tex.rgb;
    
    // Gamma Correction
    float gamma = 2.2;
    result = pow(result, vec3(1.0 / gamma));
    FragColor = vec4(result, 1.0);
}