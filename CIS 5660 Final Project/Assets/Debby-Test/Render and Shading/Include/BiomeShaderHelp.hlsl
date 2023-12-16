void CheckNormalUp_float(float3 Normal, float Error, out float Out)
{
    // false 
    Out = 0.0; 

    // check how close it can get to (0, 1, 0) up vector 
    float3 Up = (0.0, 1.0, 0.0); 
    float3 ErrorVector = (Error, Error, Error);
    float3 UpperRange = Up + ErrorVector; 
    float3 LowerRange = Up - ErrorVector;

    //if (LowerRange.y <= Normal.y && Normal.y <= UpperRange.y) {
    //    Out = 1.0; 
    //}

    if ((LowerRange.x <= Normal.x) && (Normal.x <= UpperRange.x)) {
        if ((LowerRange.y <= Normal.y) && (Normal.y <= UpperRange.y)) {
            if ((LowerRange.z <= Normal.z) && (Normal.z <= UpperRange.z)) {
                Out = 1.0; 
            }
        }
    }
}

void CheckIfGrass_float(float3 Normal, float Error, float Height, float HeightThreshold, out float Out)
{
    // false 
    Out = 0.0;

    // check how close it can get to (0, 1, 0) up vector 
    float3 Up = (0.0, 1.0, 0.0);
    float3 ErrorVector = (Error, Error, Error);
    float3 UpperRange = Up + ErrorVector;
    float3 LowerRange = Up - ErrorVector;

    if (Height >= HeightThreshold) {
        if (LowerRange.y <= Normal.y && Normal.y <= UpperRange.y) {
            Out = 1.0;
        }
    }
}

float rnd(float2 xy)
{
    return frac(sin(dot(xy, float2(12.9898 - 0.0, 78.233 + 0.0))) * (43758.5453 + 0.0));
}

float inter(float a, float b, float x)
{
    //return a*(1.0-x) + b*x; // Linear interpolation

    float f = (1.0 - cos(x * 3.1415927)) * 0.5; // Cosine interpolation
    return a * (1.0 - f) + b * f;
}

float perlin(float2 uv)
{
    float a, b, c, d, coef1, coef2, t, p;

    t = 8.0;					// Precision
    p = 0.0;								// Final heightmap value

    for (float i = 0.0; i < 8.0; i++)
    {
        a = rnd(float2(floor(t * uv.x) / t, floor(t * uv.y) / t));	    //	a----b
        b = rnd(float2(ceil(t * uv.x) / t, floor(t * uv.y) / t));		//	|    |
        c = rnd(float2(floor(t * uv.x) / t, ceil(t * uv.y) / t));		//	c----d
        d = rnd(float2(ceil(t * uv.x) / t, ceil(t * uv.y) / t));

        if ((ceil(t * uv.x) / t) == 1.0)
        {
            b = rnd(float2(0.0, floor(t * uv.y) / t));
            d = rnd(float2(0.0, ceil(t * uv.y) / t));
        }

        coef1 = frac(t * uv.x);
        coef2 = frac(t * uv.y);
        p += inter(inter(a, b, coef1), inter(c, d, coef1), coef2) * (1.0 / pow(2.0, (i + 0.6)));
        t *= 2.0;
    }
    return p;
}

void modifyHeightWithNoise_float(float noiseScale, float noiseIntensity, float height, float3 worldPos, out float Out)
{
    float noise = perlin(float2(worldPos.x, worldPos.z) * noiseScale) * noiseIntensity;
    Out = height + noise;
}

float biome_noise2D(float2 n)
{
    return frac(sin(dot(n, float2(311.7, 191.999))) * 1434.2371);
}

float biome_interpolation2D(float x, float y) {
    int intX = floor(x);
    int intY = floor(y);

    float newX = frac(x);
    float newY = frac(y);

    float fractX = smoothstep(0.0, 1.0, newX);
    float fractY = smoothstep(0.0, 1.0, newY);

    float v1 = biome_noise2D(float2(intX, intY));
    float v2 = biome_noise2D(float2(intX + 1, intY));
    float v3 = biome_noise2D(float2(intX, intY + 1));
    float v4 = biome_noise2D(float2(intX + 1, intY + 1));

    float i1 = lerp(v1, v2, fractX);
    float i2 = lerp(v3, v4, fractX);

    return lerp(i1, i2, fractY);
}

float biomeType(float x, float y) {
    float scale = 140.0;
    float noiseValue = biome_interpolation2D(x / scale, y / scale);

    return noiseValue;
}

float isCliffBiome(float x, float y) {
    float biomeValue = biomeType(x, y);
    float threshold = 0.5;

    float blend = smoothstep(0.45, 0.68, abs(biomeValue - threshold) * 2.0);

    return lerp(0, 1, blend);
}

void getBiome_float(float3 terrainSize, float cellSize, float noiseScale, float2 noiseOffset, float3 localPos, out float isCliff)
{
    int xSegments = (int)(terrainSize.x / cellSize);
    int zSegments = (int)(terrainSize.y / cellSize);

    float xStep = terrainSize.x / (float)xSegments;
    float zStep = terrainSize.y / (float)zSegments;

    float localPosXStepNormalized = localPos.x / xStep;
    float localPosZStepNormalized = localPos.y / zStep;

    float noiseX = noiseScale * localPosXStepNormalized / (float)xSegments + noiseOffset.x;
    float noiseZ = noiseScale * localPosZStepNormalized / (float)zSegments + noiseOffset.y;

    isCliff = isCliffBiome(noiseX, noiseZ);
}