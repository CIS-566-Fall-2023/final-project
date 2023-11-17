#version 300 es
precision highp float;

uniform sampler2D u_ObstacleBuffer;
uniform float u_ShowObs;

in vec4 fs_Pos;
in vec2 sampleCoords;

out vec4 out_Col;

void main()
{
    vec4 tex_color = texture(u_ObstacleBuffer, sampleCoords);
    vec2 normal = 2.0 * tex_color.rg - 1.0;

    if (dot(normal, normal) < 0.1) {
        discard; // abandon coloring 
    }

    if (u_ShowObs == 1.0)
    {
        out_Col = vec4(vec3(0.32, 0.4, 0.8), 1.0);
    }
    else 
    {
        discard;
    }

}