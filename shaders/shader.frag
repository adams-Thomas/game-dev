#version 330 core
            
in vec2 frag_textCoords;

out vec4 out_color;

 void main() {
    out_color = vec4(frag_textCoords.x, frag_textCoords.y, 0.0, 1.0);
}