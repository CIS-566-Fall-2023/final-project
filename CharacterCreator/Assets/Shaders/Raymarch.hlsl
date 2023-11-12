#define MAX_SDF_OBJECTS 256
#define MAX_ITERS 64
#define MAX_DIST 1000
#define EPSILON float3(0.0f, 0.001f, 0.0f)

float SDFType[MAX_SDF_OBJECTS];
float4 SDFPositions[MAX_SDF_OBJECTS];
float SDFSizes[MAX_SDF_OBJECTS];
float SDFBlendFactor[MAX_SDF_OBJECTS];
float SDFBlendOperation[MAX_SDF_OBJECTS];
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
	return max(a, -b);
}

float intersect(float a, float b)
{
	return max(a, b);
}

float sdfBox(float3 pos, float3 center, float3 size)
{
	float3 q = abs(pos - center) - size;
	return length(max(q, 0.0)) + min(max(q.x, max(q.y, q.z)), 0.0);
}

float sdfSphere(float3 pos, float3 center, float radius)
{
	return (lengthSqr(pos - center) - radius * radius);
}

float sceneSdf(float3 pos)
{
	float sdf = FLT_MAX;
	for (int i = 0; i < SDFCount; i++)
	{
		float3 center = SDFPositions[i];
		float size = SDFSizes[i];
		int blendOp = SDFBlendOperation[i];
		int sdfType = SDFType[i];

		float curSdf = 0.0f;
		if (sdfType == 0)
		{
			curSdf = sdfSphere(pos, center, size);
		}
		else
		{
			curSdf = sdfBox(pos, center, size);
		}

		if (blendOp == 0)
		{
			sdf = smoothMin(sdf, curSdf, SDFBlendFactor[i]);
		}
		else if (blendOp == 1)
		{
			sdf = subtract(sdf, curSdf);
		}
		else
		{
			sdf = intersect(sdf, curSdf);
		}
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
	float dist = 0.00001f;
	float4 outColor = float4(0.0f, 0.0f, 0.0f, 0.0f);

	for (int i = 0; i < MAX_ITERS; i++)
	{
		float3 p = rayOrigin + rayDirection * dist;

		float m = sceneSdf(p);

		if (m <= 0.00001f)
		{
			// hit the sphere
			outColor = float4(1, 1, 1, 1);
			normal = calculateNormal(p);
			break;
		}

		dist += m;
		if (dist >= MAX_DIST)
		{
			break;
		}
	}

	color = outColor;
}