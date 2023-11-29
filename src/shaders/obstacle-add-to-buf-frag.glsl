#version 300 es
precision highp float;

uniform vec2 u_ObstaclePos;
uniform float u_ObstacleSize;
uniform float u_StarObs;
uniform vec2 u_Dimensions;

in vec4 fs_Pos;
in vec2 sampleCoords;

out vec4 out_Col;

// source: IQ Star SDF
// signed distance to a n-star polygon with external angle en
float sdStar(in vec2 p, in float r, in int n, in float m) // m=[2,n]
{
    // these 4 lines can be precomputed for a given shape
    float an = 3.141593/float(n);
    float en = 3.141593/m;
    vec2  acs = vec2(cos(an),sin(an));
    vec2  ecs = vec2(cos(en),sin(en)); // ecs=vec2(0,1) and simplify, for regular polygon,

    // symmetry (optional)
    p.x = abs(p.x);
    
    // reduce to first sector
    float bn = mod(atan(p.x,p.y),2.0*an) - an;
    p = length(p)*vec2(cos(bn),abs(sin(bn)));

    // line sdf
    p -= r*acs;
    p += ecs*clamp( -dot(p,ecs), 0.0, r*acs.y/ecs.y);
    return length(p)*sign(p.x);
}

void main()
{
    vec2 size = vec2(u_ObstacleSize / u_Dimensions.x, u_ObstacleSize / u_Dimensions.y);
    vec2 fromCenter = 2.0 * (sampleCoords - u_ObstaclePos) / size;

    if (u_StarObs > 0.0) // Star Shaped Obstacles
    {
        float numSides = 5.0;
        float w = 2.0 + 0.4* (numSides-2.0); // angle divisor
        float d = sdStar(fromCenter, 0.7, int(numSides), w);
        if (d < 0.0) {
            vec2 normal = normalize(fromCenter);
            out_Col = vec4(0.5 * normal + 0.5, 0, 1);
        }
    }
    else // Circle Shaped Obstacles
    {
        if (dot(fromCenter, fromCenter) < 1.0)
        {
            vec2 normal = normalize(fromCenter);
            out_Col = vec4(0.5 * normal + 0.5, 0, 1);
        }
    }
}