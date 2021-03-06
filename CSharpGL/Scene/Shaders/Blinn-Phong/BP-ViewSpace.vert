﻿// Blinn-Phong-ViewSpace.vert
#version 150
in vec3 inPosition;
in vec3 inNormal;
in vec2 inTexCoord;

// Declare an interface block.
out VS_OUT {
    vec3 position;
	vec3 normal;
	vec2 texCoord;
} vs_out;

uniform mat4 projectionMat;
uniform mat4 viewMat;
uniform mat4 modelMat;
uniform mat4 normalMat; // transpose(inverse(viewMat * modelMat));

void main() {
    vec4 viewPos = viewMat * modelMat * vec4(inPosition, 1.0);
    gl_Position = projectionMat * viewPos;
	vs_out.position = viewPos.xyz;
	vs_out.normal = (normalMat * vec4(inNormal, 0)).xyz;
	vs_out.texCoord = inTexCoord;
}
