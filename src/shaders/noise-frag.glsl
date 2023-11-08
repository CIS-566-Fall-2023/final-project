#version 300 es

precision highp float;

uniform vec4 u_Color; // The color with which to render this instance of geometry.
uniform float u_Time;

// These are the interpolated values out of the rasterizer, so you can't know
// their specific values without knowing the vertices that contributed to them
in vec4 fs_Nor;
in vec4 fs_LightVec;
in vec4 fs_Col;
in vec4 fs_Pos;

out vec4 out_Col; // This is the final output color that you will see on your

vec3 random3(vec3 p)
{
    return fract(sin(vec3(dot(p, vec3(127.1, 311.7, 191.999)),
                           dot(p, vec3(269.5, 183.3, 191.999)),
                           dot(p, vec3(420.6, 631.2, 191.999))))
                   * 43758.5453);
}

float surflet(vec3 p, vec3 gridPoint) {
    // Compute the distance between p and the grid point along each axis, and warp it with a
    // quintic function so we can smooth our cells
    vec3 t2 = abs(p - gridPoint);
    vec3 t = vec3(1) - 6.0 * pow(t2, vec3(5)) + 15.0 * pow(t2, vec3(4)) - 10.0 * pow(t2, vec3(3));
    
    // Get the random vector for the grid point 
    vec3 gradient = random3(gridPoint) * 12.0 - vec3(1, 1, 1);
    
    // Get the vector from the grid point to P
    vec3 diff = p - gridPoint;
    
    // Get the value of our height field by dotting grid->P with our gradient
    float height = dot(diff, gradient);
    
    // Scale our height field (i.e. reduce it) by our polynomial falloff function
    return height * t.x * t.y * t.z;
}

float perlinNoise3D(vec3 p) {
	float surfletSum = 0.0;
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

void main()
{
    float t = sin(u_Time * 0.01);
    float a = perlinNoise3D(fs_Pos.xyz);
    float b = perlinNoise3D(fs_Pos.xyz * vec3(5.0));
    float c = perlinNoise3D(fs_Pos.xyz * vec3(10.0));

    vec3 inverseColor = vec3(255.0- u_Color.r, 255.0 - u_Color.g, 255.0 - u_Color.b) / 255.0;
    vec4 diffuseColor1 = vec4(a * u_Color.rgb, 1.0);
    vec4 diffuseColor2 = vec4(b * inverseColor, 1.0);
    vec4 diffuseColor3 = vec4(c * u_Color.rgb, 1.0);
    vec4 diffuseColor = diffuseColor1 + 0.25 * t * diffuseColor2 + diffuseColor3;



    float diffuseTerm = dot(normalize(fs_Nor), normalize(fs_LightVec));

    float ambientTerm = 0.2;

    float lightIntensity = diffuseTerm + ambientTerm;

    out_Col = vec4(diffuseColor.rgb * lightIntensity, diffuseColor1.a);
}
