#ifndef RAYMARCHSHADERINCLUDE
#define RAYMARCHSHADERINCLUDE

#define MAX_SDF_OBJECTS 256
#define MAX_ITERS 512
#define MAX_DIST 1000
#define MIN_DIST 0.001
#define EPSILON float3(0.0f, MIN_DIST, 0.0f)

float SDFType[MAX_SDF_OBJECTS];
//float4 SDFPositions[MAX_SDF_OBJECTS];
float4 SDFData[MAX_SDF_OBJECTS];
float SDFBlendFactor[MAX_SDF_OBJECTS];
float SDFBlendOperation[MAX_SDF_OBJECTS];
float4x4 SDFTransformMatrices[MAX_SDF_OBJECTS];
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
float add(float a, float b, float k)
{
	float h = max(k - abs(a - b), 0.0) / k;
	return min(a, b) - h * h * k * 0.25f;
}

float subtract(float a, float b, float k)
{
	//return max(a, -b);
	float h = clamp(0.5 - 0.5 * (a + b) / k, 0.0, 1.0);
	return lerp(a, -b, h) + k * h * (1.0 - h);
}

float intersect(float a, float b)
{
	return max(a, b);
}

float sdfBox(float3 pos, float3 size)
{
	float3 q = abs(pos) - size;
	return length(max(q, 0.0)) + min(max(q.x, max(q.y, q.z)), 0.0);
}

float sdfSphere(float3 pos, float radius)
{
	return (lengthSqr(pos) - radius * radius);
}

float sdfTorus(float3 pos, float2 size)
{
	float2 q = float2(length(pos.xz) - size.x, pos.y);
	return length(q) - size.y;
}

float sdfCylinder(float3 pos, float height, float radius)
{
	float2 d = abs(float2(length(pos.xz), pos.y)) - float2(radius, height);
	return min(max(d.x, d.y), 0.0) + length(max(d, 0.0));
}

float sdfCapsule(float3 pos, float height, float radius)
{
	float halfHeight = height * 0.5;
	pos.y -= clamp(pos.y, -halfHeight, halfHeight);		// modified from IQ's version: move this to the center of axis!
	return length(pos) - radius;
}

float sdfOctahedron(float3 pos, float size)
{
	pos = abs(pos);
	return (pos.x + pos.y + pos.z - size) * 0.57735027;
}

float sdfCone(float3 pos, float angle, float height)
{
	float2 c = float2(sin(angle), cos(angle));
	float q = length(pos.xz);
	//pos.y -= height;
	return max(dot(c.xy, float2(q, pos.y)), -height - pos.y);
}

float sdfRoundness(float sdf, float roundness)
{
	return sdf - roundness;
}

float sceneSdf(float3 pos)
{
	float sdf = FLT_MAX;
	float3 posTransformed;
	for (int i = 0; i < SDFCount; i++)
	{
		float size = SDFData[i].x;
		int blendOp = SDFBlendOperation[i];
		int sdfType = SDFType[i];

		float4x4 transform = SDFTransformMatrices[i];
		posTransformed = mul(transform, float4(pos, 1.0)).xyz;

		float roundness = SDFData[i].w;	// .w is always roundness

		float curSdf = 0.0f;
		if (sdfType == 0)
		{
			curSdf = sdfSphere(posTransformed, size);
		}
		else if (sdfType == 1)
		{
			curSdf = sdfBox(posTransformed, size);
		}
		else if (sdfType == 2)
		{
			curSdf = sdfTorus(posTransformed, float2(size, SDFData[i].y));
		}
		else if (sdfType == 3)
		{
			curSdf = sdfCylinder(posTransformed, SDFData[i].y, size);
		}
		else if (sdfType == 4)
		{
			curSdf = sdfCapsule(posTransformed, SDFData[i].y, size);
		}
		else if (sdfType == 5)
		{
			curSdf = sdfOctahedron(posTransformed, size);
		}
		else if (sdfType == 6)
		{
			curSdf = sdfCone(posTransformed, size, SDFData[i].y);
		}

		if (roundness > 0.0)
		{
			curSdf = sdfRoundness(curSdf, roundness);
		}

		if (blendOp == 0)
		{
			sdf = add(sdf, curSdf, SDFBlendFactor[i]);
		}
		else if (blendOp == 1)
		{
			sdf = subtract(sdf, curSdf, SDFBlendFactor[i]);
		}
		else
		{
			sdf = intersect(sdf, curSdf);
		}
	}

	return sdf;
}

float3 CalculateNormal(float3 pos)
{
	return normalize(float3(sceneSdf(pos + EPSILON.yxx) - sceneSdf(pos - EPSILON.yxx),
							sceneSdf(pos + EPSILON.xyx) - sceneSdf(pos - EPSILON.xyx),
							sceneSdf(pos + EPSILON.xxy) - sceneSdf(pos - EPSILON.xxy)));
}

void Raymarch_float(float3 rayOriginObjectSpace, float3 rayDirectionObjectSpace, out float4 outColor, out float3 objectSpaceNormal)
{
	float dist = MIN_DIST;
	outColor = float4(0.0f, 0.0f, 0.0f, 0.0f);

	for (int i = 0; i < MAX_ITERS; i++)
	{
		float3 p = rayOriginObjectSpace + rayDirectionObjectSpace * dist;

		float m = sceneSdf(p);

		if (m <= MIN_DIST)
		{
			// hit the sphere
			outColor = float4(1, 1, 1, 1);
			objectSpaceNormal = CalculateNormal(p);
			break;
		}

		dist += m;
		if (dist >= MAX_DIST)
		{
			break;
		}
	}
}

void GetLighting_float(float3 worldSpaceNormal, out float3 outColor)
{
#if defined(SHADERGRAPH_PREVIEW)
	outColor = float3(1, 1, 1);
#else
	Light mainLight = GetMainLight();

	half NdotL = saturate(dot(worldSpaceNormal, mainLight.direction));
	NdotL += 0.2;	// ambient term
	outColor = mainLight.color * NdotL;
#endif
}

#endif