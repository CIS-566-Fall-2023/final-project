#version 300 es

// This is a fragment shader. If you've opened this file first, please
// open and read lambert.vert.glsl before reading on.
// Unlike the vertex shader, the fragment shader actually does compute
// the shading of geometry. For every pixel in your program's output
// screen, the fragment shader is run for every bit of geometry that
// particular pixel overlaps. By implicitly interpolating the position
// data passed into the fragment shader by the vertex shader, the fragment shader
// can compute what color to apply to its pixel based on things like vertex
// position, light position, and vertex color.
precision highp float;

uniform vec4 u_Color; // The color with which to render this instance of geometry.
uniform vec4 u_MiddleColor;
uniform vec4 u_FrontColor;
uniform float u_Time;
uniform int u_Magic;

// These are the interpolated values out of the rasterizer, so you can't know
// their specific values without knowing the vertices that contributed to them
in vec4 fs_Nor;
in vec4 fs_LightVec;
in vec4 fs_Col;
in vec4 fs_Pos;


out vec4 out_Col; // This is the final output color that you will see on your
                  // screen for the pixel that is currently being processed.

float bias(float b, float t)
{
    return pow(t, log(b) / log(0.5));
}

float triangle_wave(float x, float freq, float amp)
{
    return abs(mod((x * freq), amp) - (0.5 * amp));
}

void main()
{
    vec3 yellow = u_MiddleColor.rgb;
    vec3 red = u_Color.rgb;
    vec3 blue = u_FrontColor.rgb;
    
    // SMOOTHSTEP
    float val = clamp(smoothstep(0.6, 0.9, fs_Pos.z-0.25),0.0,1.0);
    val = bias(0.4, val);
    vec3 col = mix(yellow, blue, val);
    float t = triangle_wave(u_Time * 0.005, 0.25, 2.0);
    col = mix(col, red, t);

    float val2;
    if (u_Magic == 1) {
        val2 = clamp(smoothstep(0.1, 0.8, (fs_Pos.z + 0.8) * 0.5),0.0,1.0);
    }
    else {
        val2 = clamp(smoothstep(0.1, 0.8, (fs_Pos.z + 0.5) * 0.5),0.0,1.0);
    }
    col = mix(red, col, val2);

    // Calculate the diffuse term for Lambert shading
    float diffuseTerm = dot(normalize(fs_Nor), normalize(fs_LightVec)) * 0.75;
    float ambientTerm = 0.6;
    float lightIntensity = diffuseTerm + ambientTerm;  

    // Compute final shaded color
    out_Col = vec4(col * lightIntensity, u_Color.a);
}


