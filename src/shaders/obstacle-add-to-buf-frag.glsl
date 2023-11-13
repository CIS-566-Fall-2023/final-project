#version 300 es
precision highp float;

uniform vec2 u_ObstaclePos;
uniform sampler2D u_Texture;

in vec4 fs_Pos;
in vec2 sampleCoords;

out vec4 out_Col;

void main()
{
    vec2 fromCenter = 2.0 * (sampleCoords - u_ObstaclePos);
    
    if (dot(fromCenter, fromCenter) < 1.0)
    {
        vec2 normal = normalize(fromCenter);
        out_Col = vec4(0.5 * normal + 0.5, 0, 1);
    }
    else 
    {
        out_Col = texture(u_Texture, sampleCoords);
    }

}