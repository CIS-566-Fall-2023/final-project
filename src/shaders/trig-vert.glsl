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

uniform float u_Time;

uniform float u_Explosivity;

uniform float u_Sound[128];

in vec4 vs_Pos;             // The array of vertex positions passed to the shader

in vec4 vs_Nor;             // The array of vertex normals passed to the shader

in vec4 vs_Col;             // The array of vertex colors passed to the shader.

in float vs_Sound;


out vec4 fs_Nor;            // The array of normals that has been transformed by u_ModelInvTr. This is implicitly passed to the fragment shader.
out vec4 fs_LightVec;       // The direction in which our virtual light lies, relative to each vertex. This is implicitly passed to the fragment shader.
out vec4 fs_Col;            // The color of each vertex. This is implicitly passed to the fragment shader.
out vec4 fs_Pos;
out float fs_Sound;

const vec4 lightPos = vec4(5, 5, 3, 1); //The position of our virtual light, which is used to compute the shading of
                                        //the geometry in the fragment shader.

vec3 random3(vec3 p) {
    return fract(sin(vec3(dot(p,vec3(12.71, 31.17, 56.35)),
                              dot(p,vec3(26.95, 18.33, 45.16)),
                              dot(p, vec3(42.06, 63.12, 95.28))
                        )) * 458.5453f);

}

vec3 pow3(vec3 v, float p) {
    return vec3(pow(v[0], p),pow(v[1], p),pow(v[2], p));
}

float surflet(vec3 p, vec3 gridPoint) {
    // Compute the distance between p and the grid point along each axis, and warp it with a
    // quintic function so we can smooth our cells
    vec3 t2 = abs(p - gridPoint);
    vec3 t = vec3(1.f) - 6.f * pow3(t2, 5.f) + 15.f * pow3(t2, 4.f) - 10.f * pow3(t2, 3.f);
    // Get the random vector for the grid point (assume we wrote a function random2
    // that returns a vec2 in the range [0, 1])
    vec3 gradient = random3(gridPoint) * 2. - vec3(1., 1., 1.);
    // Get the vector from the grid point to P
    vec3 diff = p - gridPoint;
    // Get the value of our height field by dotting grid->P with our gradient
    float height = dot(diff, gradient);
    // Scale our height field (i.e. reduce it) by our polynomial falloff function
    return height * t.x * t.y * t.z;
}

float perlinNoise3D(vec3 p) {
	float surfletSum = 0.f;
	// Iterate over the four integer corners surrounding uv
	for(int dx = 0; dx <= 1; ++dx) {
		for(int dy = 0; dy <= 1; ++dy) {
			for(int dz = 0; dz <= 1; ++dz) {
				surfletSum += surflet(p, floor(p) + vec3(dx, dy, dz));
			}
		}
	}
	return surfletSum;
}

float fbm(int c, vec3 p) {
    float normalizer = 0.f;
    float mult = 1.f;
    float res = 0.f;
    for(int i = 0; i < c; i++) {
        res += mult*perlinNoise3D(p*mult);
        normalizer += mult;
        mult/=2.f;
    }
    return res/normalizer;
}
 
void main()
{
                      // Pass the vertex colors to the fragment shader for interpolation
    float t = u_Time/100.f;
    vec3 pdelta = vec3(vs_Pos) + vec3(sin(t), cos(t), sin(t)*cos(t));

    float i = fbm(5, vec3(vs_Pos)) * 40.f + 40.f;
    int i1 = int(floor(i));
    int i2 = int(ceil(i));
    float f = i-float(i1);

    i1 = (i1+128)%128;
    i2 = (i2+128)%128;

    float soundMod = max(1.f + u_Explosivity * (u_Sound[i1] * f + u_Sound[i2] * (1.0-f) - 150.f)/320.f, 0.6);
    fs_Col = vs_Col; 
          
    
    vec4 newPos = vec4(soundMod* vec3(vs_Pos) * (1.0 + 0.5*fbm(10, 5.f*pdelta)), 1.f);


    fs_Pos = newPos;

    mat3 invTranspose = mat3(u_ModelInvTr);
    fs_Nor = vec4(invTranspose * vec3(vs_Nor), 0);          // Pass the vertex normals to the fragment shader for interpolation.
                                                            // Transform the geometry's normals by the inverse transpose of the
                                                            // model matrix. This is necessary to ensure the normals remain
                                                            // perpendicular to the surface after the surface is transformed by
                                                            // the model matrix.


    vec4 modelposition = u_Model * newPos;   // Temporarily store the transformed vertex positions for use below

    fs_LightVec = lightPos - modelposition;  // Compute the direction in which the light source lies

    gl_Position = u_ViewProj * modelposition;// gl_Position is a built-in variable of OpenGL which is
                                             // used to render the final positions of the geometry's vertices

    fs_Sound = 1.f;//soundMode;
}
