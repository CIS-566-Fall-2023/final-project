#version 300 es
precision highp float;

in vec4 fs_Pos;
in vec2 fromCenter;

out vec4 out_Col;

void main()
{    
    if (dot(fromCenter, fromCenter) < 2.0)
    {
        discard;
    }
    else 
    {
        vec2 normal = normalize(fromCenter);
        out_Col = vec4(0.5 * normal + 0.5, 0, 1);
    }

}