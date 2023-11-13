#version 300 es
precision highp float;

uniform mat4 u_ViewProj;
uniform vec2 u_ObstaclePos;

in vec4 vs_Pos;
out vec4 fs_Pos;

out vec2 fromCenter;

void main()
{
    fromCenter = 2.0 * vs_Pos.xy;

    vec2 obsCenter = u_ObstaclePos + vec2(0.1) * vs_Pos.xy;
    // remap
    gl_Position = vec4(2.0 * obsCenter - 1.0, 0.0, 1.0);
}