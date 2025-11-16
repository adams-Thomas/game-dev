#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTextureCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

// Stores and outputs the data we want to the fragment shader
out vec2 frag_textCoords;

void main()
{
    gl_Position = projection * view * model * vec4(aPosition, 1.0);
    frag_textCoords = aTextureCoord;
}