#version 300 es
precision highp float;

uniform mat4 u_ViewProj;

in vec4 vs_Pos;
out vec4 fs_Pos;

out vec2 sampleCoords;

void main()
{
    sampleCoords = vs_Pos.xy;
    // remap
    gl_Position = vec4(2.0 * vs_Pos.yx - 1.0, 0.0, 1.0);
}