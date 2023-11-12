#define MAX_SDF_OBJECTS 256
#define MAX_ITERS 64
#define MAX_DIST 1000
#define EPSILON float3(0.0f, 0.001f, 0.0f)

float4 SDFPositions[MAX_SDF_OBJECTS];
float SDFSizes[MAX_SDF_OBJECTS];
float SDFCount;

struct Ray
{
	float3 origin;
	float3 direction;
};

float lengthSqr(float3 vec)
{
	return vec.x * vec.x + vec.y * vec.y + vec.z * vec.z;
}

// from IQ
float smoothMin(float a, float b, float k)
{
	float h = max(k - abs(a - b), 0.0) / k;
	return min(a, b) - h * h * k * 0.25f;
}

float subtract(float a, float b)
{
	return max(-a, b);
}

float intersect(float a, float b)
{
	return max(a, b);
}

float sdfSphere(float3 pos, float3 center, float radius)
{
	return (lengthSqr(pos - center) - radius);
}

float sceneSdf(float3 pos)
{
	float sdf = FLT_MAX;
	for (int i = 0; i < SDFCount; i++)
	{
		float3 center = SDFPositions[i];
		float radius = SDFSizes[i];

		sdf = smoothMin(sdf, sdfSphere(pos, center, radius), 0.1f);
		//sdf = intersect(sdf, sdfSphere(pos, center, radius));
	}

	return sdf;
}

float calculateNormal(float3 pos)
{
	return normalize(float3(sceneSdf(pos + EPSILON.yxx) - sceneSdf(pos - EPSILON.yxx),
							sceneSdf(pos + EPSILON.xyx) - sceneSdf(pos - EPSILON.xyx),
							sceneSdf(pos + EPSILON.xxy) - sceneSdf(pos - EPSILON.xxy)));
}

void Raymarch_float(float3 rayOrigin, float3 rayDirection, out float4 color, out float3 normal)
{
	float dist = 0.01f;
	for (int i = 0; i < MAX_ITERS; i++)
	{
		float3 p = rayOrigin + rayDirection * dist;

		float m = sceneSdf(p);

		if (m <= 0.0f)
		{
			// hit the sphere
			color = float4(1, 1, 1, 1);
			normal = calculateNormal(p);
			return;
		}

		dist += 0.01f;
		if (dist >= MAX_DIST)
		{
			break;
		}
	}

	color = float4(0, 0, 0, 0);
}