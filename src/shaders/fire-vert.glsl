#version 300 es

//This is a vertex shader. While it is called a "shader" due to outdated conventions, this file
//is used to apply matrix transformations to the arrays of vertex data passed to it.
//Since this code is run on your GPU, each vertex is transformed simultaneously.
//If it were run on your CPU, each vertex would have to be processed in a FOR loop, one at a time.
//This simultaneous transformation allows your program to run much faster, especially when rendering
//geometry with millions of vertices.

uniform mat4 u_Model;       // The matrix that defines the transformation of the
                            // object we're rendering. In this assignment,
                            // this will be the result of traversing your scene graph.

uniform mat4 u_ModelInvTr;  // The inverse transpose of the model matrix.
                            // This allows us to transform the object's normals properly
                            // if the object has been non-uniformly scaled.

uniform mat4 u_ViewProj;    // The matrix that defines the camera's transformation.
                            // We've written a static matrix for you to use for HW2,
                            // but in HW3 you'll have to generate one yourself

in vec4 vs_Pos;             // The array of vertex positions passed to the shader

in vec4 vs_Nor;             // The array of vertex normals passed to the shader

in vec4 vs_Col;             // The array of vertex colors passed to the shader.

uniform float u_Time;
uniform float u_Speed;
uniform float u_TailSize;

out vec4 fs_Pos;
out vec4 fs_Nor;            // The array of normals that has been transformed by u_ModelInvTr. This is implicitly passed to the fragment shader.
out vec4 fs_LightVec;       // The direction in which our virtual light lies, relative to each vertex. This is implicitly passed to the fragment shader.
out vec4 fs_Col;            // The color of each vertex. This is implicitly passed to the fragment shader.


const vec4 lightPos = vec4(0, 0, 20, 1); //The position of our virtual light, which is used to compute the shading of
                                        //the geometry in the fragment shader.


float noise3D(vec3 p)
{
        return fract(sin(dot(p, vec3(127.1,269.5, 59.137))) *
                     43758.5453);
}

float interpNoise3D(float x, float y, float z)
{
    int intX = int(floor(x));
    float fractX = fract(x);
    int intY = int(floor(y));
    float fractY = fract(y);
    int intZ = int(floor(z));
    float fractZ = fract(z);

    float v1 = noise3D(vec3(intX, intY, intZ));
    float v2 = noise3D(vec3(intX + 1, intY, intZ));
    float v3 = noise3D(vec3(intX, intY + 1, intZ));
    float v4 = noise3D(vec3(intX + 1, intY + 1, intZ));
    
    float v5 = noise3D(vec3(intX, intY, intZ+1));
    float v6 = noise3D(vec3(intX + 1, intY, intZ+1));
    float v7 = noise3D(vec3(intX, intY + 1, intZ+1));
    float v8 = noise3D(vec3(intX + 1, intY + 1, intZ+1));

    float i1 = mix(v1, v2, fractX);
    float i2 = mix(v3, v4, fractX);
    float i3 = mix(v5, v6, fractX);
    float i4 = mix(v7, v8, fractX);
    
    float j1 = mix(i1, i2, fractY);
    float j2 = mix(i3, i4, fractY);
    
    return mix(j1, j2, fractZ);
}

float fbm(float x, float y, float z, float amp, float freq)
{
    float total = 0.0;
    float persistence = 0.5f;
    int octaves = 8;

    for(int i = 1; i <= octaves; i++) {
        total += interpNoise3D(x * freq,
                               y * freq,
                               z * freq) * amp;

        freq *= 2.f;
        amp *= persistence;
    }
    return total;
}

float sawtooth_wave(float x, float f, float a)
{
    return (x * f - floor(x * f)) * a;
}

void main()
{
    fs_Col = vs_Col;                       // Pass the vertex colors to the fragment shader for interpolation
    fs_Pos = vs_Pos;

    mat3 invTranspose = mat3(u_ModelInvTr);
    fs_Nor = vec4(invTranspose * vec3(vs_Nor), 0);          // Pass the vertex normals to the fragment shader for interpolation.
                                                            // Transform the geometry's normals by the inverse transpose of the
                                                            // model matrix. This is necessary to ensure the normals remain
                                                            // perpendicular to the surface after the surface is transformed by
                                                            // the model matrix.


    vec4 modelposition = u_Model * vs_Pos;   // Temporarily store the transformed vertex positions for use below

    float t = cos(u_Time * 0.01);

    // use FBM
    // take the noise value as a height
    float amp1 = 3.0 + sin(t + 0.32) * 0.1;
    float freq1 = 1.5 + sin(t) * 0.34;
    float wind = abs((sin(t) + 2.0) * 0.5);
    wind = sawtooth_wave(u_Time * 0.005, u_Speed, 5.0); // 0.025 to .5
    float d1 = fbm(modelposition.x, modelposition.y, modelposition.z + 1.2 * wind, 0.5, 4.0);

    fs_LightVec = lightPos - modelposition;  // Compute the direction in which the light source lies

    if (modelposition.z > 0.0)
    {
        modelposition = d1 * fs_Nor + modelposition;
    }
    else
    {
        vec4 tail = (u_TailSize * abs(modelposition.z) + 1.0) * d1 * fs_Nor + modelposition; 
        modelposition = (1.2 * abs(modelposition.z) + 1.0) * d1 * fs_Nor + modelposition; 
        modelposition.z = tail.z;
    }

    gl_Position = u_ViewProj * modelposition;// gl_Position is a built-in variable of OpenGL which is
                                             // used to render the final positions of the geometry's vertices
}
