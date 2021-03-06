﻿// Blinn-Phong-WorldSpace.frag
#version 150

struct Light {
    vec3 position;   // for directional light, meaningless.
    vec3 diffuse;
    vec3 specular;
    float constant;  // Attenuation.constant.
    float linear;    // Attenuation.linear.
    float quadratic; // Attenuation.quadratic.
	// direction from outer space to light source.
	vec3 direction;  // for point light, meaningless.
	// Note: We assume that spot light's angle ranges from 0 to 180 degrees.
	float cutOff;    // for spot light, cutOff. for others, meaningless.
};

struct Material {
    vec3 diffuse;
    vec3 specular;
    float shiness;
};

uniform Light light;

uniform Material material;

uniform vec3 eyePos;

uniform bool blinn = true;
uniform int lightUpRoutine; // 0: point light; 1: directional light; 2: spot light.

in VS_OUT {
    vec3 position;
	vec3 normal;
} fs_in;

void PointLightUp(Light light, out float diffuse, out float specular) {
    vec3 Distance = light.position - fs_in.position;
	vec3 lightDir = normalize(Distance);
	vec3 normal = normalize(fs_in.normal); 
	float distance = length(Distance);
	float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * distance * distance);
	
	// Diffuse color
    diffuse = max(dot(lightDir, normal), 0) * attenuation;

	// Specular color
	vec3 eyeDir = normalize(eyePos - fs_in.position);
	float spec;
	if (blinn) {
	    vec3 halfwayDir = normalize(lightDir + eyeDir);
		spec = pow(max(dot(normal, halfwayDir), 0.0), material.shiness);
	}
	else {
	    vec3 reflectDir = reflect(-lightDir, normal);
		spec = pow(max(dot(eyeDir, reflectDir), 0.0), material.shiness);
	}
    specular = spec * attenuation;
}

void DirectionalLightUp(Light light, out float diffuse, out float specular) {
	vec3 lightDir = normalize(light.direction);
	vec3 normal = normalize(fs_in.normal); 
	
	// Diffuse color
    diffuse = max(dot(lightDir, normal), 0);

	// Specular color
	vec3 eyeDir = normalize(eyePos - fs_in.position);
	float spec;
	if (blinn) {
	    vec3 halfwayDir = normalize(lightDir + eyeDir);
		spec = pow(max(dot(normal, halfwayDir), 0.0), material.shiness);
	}
	else {
	    vec3 reflectDir = reflect(-lightDir, normal);
		spec = pow(max(dot(eyeDir, reflectDir), 0.0), material.shiness);
	}
    specular = spec;
}

// Note: We assume that spot light's angle ranges from 0 to 180 degrees.
void SpotLightUp(Light light, out float diffuse, out float specular) {
    vec3 Distance = light.position - fs_in.position;
	vec3 lightDir = normalize(Distance);
	vec3 centerDir = normalize(light.direction);
	float c = dot(lightDir, centerDir);// cut off at this point.
	if (c < 0 // current point is behind the spot light.
	    || 2 * c * c - 1 < light.cutOff) { // current point is outside of the cut off edge. 
		diffuse = 0; specular = 0;
	}
	else {
		vec3 normal = normalize(fs_in.normal); 
		float distance = length(Distance);
		float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * distance * distance);
	
		// Diffuse color
		diffuse = max(dot(lightDir, normal), 0) * attenuation;

		// Specular color
		vec3 eyeDir = normalize(eyePos - fs_in.position);
		float spec;
		if (blinn) {
			vec3 halfwayDir = normalize(lightDir + eyeDir);
			spec = pow(max(dot(normal, halfwayDir), 0.0), material.shiness);
		}
		else {
			vec3 reflectDir = reflect(-lightDir, normal);
			spec = pow(max(dot(eyeDir, reflectDir), 0.0), material.shiness);
		}
		specular = spec * attenuation;
	}
}


out vec4 fragColor;

void main() {
    float diffuse = 0;
    float specular = 0;
	if (lightUpRoutine == 0) { PointLightUp(light, diffuse, specular); }
	else if (lightUpRoutine == 1) { DirectionalLightUp(light, diffuse, specular); }
	else if (lightUpRoutine == 2) { SpotLightUp(light, diffuse, specular); }
    else { diffuse = 0; specular = 0; }

	fragColor = vec4(diffuse * light.diffuse * material.diffuse + specular * light.specular * material.specular, 1.0);
}
