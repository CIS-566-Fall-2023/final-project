#ifndef NOISESHADERINCLUDE
#define NOISESHADERINCLUDE

/* =============== NOISE FUNCTIONS ================= */

// Random


float2 random2(float2 p)
{
    return frac(sin(float2(dot(p, float2(127.1f, 311.7f)),
        dot(p, float2(269.5f, 183.3f))))
        * 43758.5453f);
}


float3 random3(float3 p)
{
    return frac(sin(float3(dot(p, float3(127.1f, 311.7f, 191.999f)),
            dot(p, float3(269.5f, 183.3f, 191.999f)),
            dot(p, float3(420.6f, 631.2f, 191.999f))))
        * 43758.5453f);
}

// FBM

float noise3D(float3 p)
{
    return frac(sin(dot(p, float3(127.1, 269.5, 191.2))) *
        43758.5453);
}

float interpNoise3D(float x, float y, float z)
{
    int intX = int(floor(x));
    float fracX = frac(x);
    int intY = int(floor(y));
    float fracY = frac(y);
    int intZ = int(floor(z));
    float fracZ = frac(z);

    // 4 points on Z1 depth
    float v1 = noise3D(float3(intX, intY, intZ));
    float v2 = noise3D(float3(intX + 1, intY, intZ));

    float v3 = noise3D(float3(intX, intY + 1, intZ));
    float v4 = noise3D(float3(intX + 1, intY + 1, intZ));

    // Bilinear on Z1 depth
    float i1 = lerp(v1, v2, fracX);
    float i2 = lerp(v3, v4, fracX);
    float iz1 = lerp(i1, i2, fracY);

    // 4 points on Z2 depth
    float v5 = noise3D(float3(intX, intY, intZ + 1));
    float v6 = noise3D(float3(intX + 1, intY, intZ + 1));

    float v7 = noise3D(float3(intX, intY + 1, intZ + 1));
    float v8 = noise3D(float3(intX + 1, intY + 1, intZ + 1));

    // Bilinear on Z2 depth
    float i3 = lerp(v5, v6, fracX);
    float i4 = lerp(v7, v8, fracX);
    float iz2 = lerp(i3, i4, fracY);

    // Final trilinear
    return lerp(iz1, iz2, fracZ);
}

float fbm(float3 pos, float amp, float freq)
{
    float total = 0.0;
    float persistence = 0.5;
    int octaves = 8;

    for (int i = 1; i <= octaves; i++) {
        total += interpNoise3D(pos.x * freq,
            pos.y * freq,
            pos.z * freq) * amp;

        freq *= 2.f;
        amp *= persistence;
    }
    return total;
}

// Worley

float WorleyNoise(float2 uv)
{
    float2 uvInt = floor(uv);
    float2 uvfrac = frac(uv);
    float minDist = 1.0; // Minimum distance initialized to max.
    for (int y = -1; y <= 1; ++y) {
        for (int x = -1; x <= 1; ++x) {
            float2 neighbor = float2(float(x), float(y)); // Direction in which neighbor cell lies
            float2 pt = random2(uvInt + neighbor); // Get the Voronoi centerpoint for the neighboring cell
            float2 diff = neighbor + pt - uvfrac; // Distance between fragment coord and neighbor’s Voronoi point
            float dist = length(diff);
            minDist = min(minDist, dist);
        }
    }
    return minDist;
}

float WorleyNoise3D(float3 uvw)
{
    float3 uvInt = floor(uvw);
    float3 uvfrac = frac(uvw);
    float minDist = 1.0; // Minimum distance initialized to max.

    for (int z = -1; z <= 1; ++z) {
        for (int y = -1; y <= 1; ++y) {
            for (int x = -1; x <= 1; ++x) {
                float3 neighbor = float3(float(x), float(y), float(z)); // Direction in which neighbor cell lies
                float3 pt = random3(uvInt + neighbor); // Get the Voronoi centerpoint for the neighboring cell
                float3 diff = neighbor + pt - uvfrac; // Distance between fragment coord and neighbor’s Voronoi point
                float dist = length(diff);
                minDist = min(minDist, dist);
            }
        }
    }
    
    return minDist;
}

// Perlin

float surflet3D(float3 p, float3 gridPoint)
{
    // Compute the distance between p and the grid point along each axis, and warp it with a
    // quintic function so we can smooth our cells
    float3 t2 = abs(p - gridPoint);
    float3 t = 1 - 6 * pow(t2, 5) + 15.f * pow(t2, 4) - 10.f * pow(t2, 3);
    // Get the random vector for the grid point (assume we wrote a function random2
    // that returns a vec2 in the range [0, 1])
    float3 gradient = normalize(random3(gridPoint) * 2.f - float3(1.f, 1.f, 1.f));
    // Get the vector from the grid point to P
    float3 diff = p - gridPoint;
    // Get the value of our height field by dotting grid->P with our gradient
    float height = dot(diff, gradient);
    // Scale our height field (i.e. reduce it) by our polynomial falloff function
    return height * t.x * t.y * t.z;
}

float perlinNoise3D(float3 p)
{
    float surfletSum = 0.f;
    // Iterate over the four integer corners surrounding uv
    for (int dx = 0; dx <= 1; ++dx) {
        for (int dy = 0; dy <= 1; ++dy) {
            for (int dz = 0; dz <= 1; ++dz) {
                surfletSum += surflet3D(p, floor(p) + float3(dx, dy, dz));
            }
        }
    }

    // remap [-1,1] tp [0,1]
    surfletSum = (surfletSum + 1) * 0.5;
    return surfletSum;
}

#endif