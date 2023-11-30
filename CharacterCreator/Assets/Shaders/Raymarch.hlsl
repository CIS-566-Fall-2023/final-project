#ifndef RAYMARCHSHADERINCLUDE
#define RAYMARCHSHADERINCLUDE

#include "Common.hlsl"
#include "Texturing.hlsl"

#define RAYMARCH_CONSTANT_STEPS 0
#define RAYMARCH_SPHERE_TRACE 1

#define MAX_ITERS 1024
#define MAX_DIST 100000
#define MIN_DIST 10e-4
#define EPSILON float3(0.0f, MIN_DIST, 0.0f)

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

/// <summary>
/// Returns the smooth minimum (square) based on a k blend factor.
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <param name="k"></param>
/// <returns>float2 value. x component is the smooth minimum value, and y component is the blend factor [0,1]</returns>
float2 add_smoothMin2(float a, float b, float k)
{
	float h = max(k - abs(a - b), 0.0) / k;
	float m = h * h * 0.5;
	float s = m * k * (1.0 / 2.0);
	return (a < b) ? float2(a - s, m) : float2(b - s, m - 1.0);
}

float subtract(float a, float b, float k)
{
	float h = clamp(0.5 - 0.5 * (a + b) / k, 0.0, 1.0);
	return float2(lerp(a, -b, h) + k * h * (1.0 - h), 0);
}

float intersect(float a, float b)
{
	return float2(max(a, b), 0);
}

float sdfBox(float3 pos, float3 size)
{
	if (pos.y > 0.1)
	{
		pos.y += sin(10 * pos.x) * sin(20 * pos.z) * 0.05;
	}
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

// Version that only calculates sdf minDist
float sceneSdf(float3 pos)
{
	float3 posTransformed;
	float2 sdf = float2(FLT_MAX, 1.0);

	for (int i = 0; i < SDFCount; i++)
	{
		float size = SDFData[i].x;
		int blendOp = SDFBlendOperation[i];
		int sdfType = SDFType[i];

		float4x4 transform = SDFTransformMatrices[i];
		posTransformed = mul(transform, float4(pos, 1.0)).xyz;

		float roundness = SDFData[i].w;	// .w is always roundness

		float curSdf = 0.0f;
		float blendT = 0.0f;

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

		if (blendOp == 0)	// add
		{
			sdf = add_smoothMin2(sdf, curSdf, SDFBlendFactor[i]);
			blendT = sdf.y;
		}
		else if (blendOp == 1)	// subtract
		{
			sdf = subtract(sdf, curSdf, SDFBlendFactor[i]);
			blendT = sdf.y;
		}
		else if (blendOp == 2)	// intersect
		{
			sdf = intersect(sdf, curSdf);
			blendT = sdf.y;
		}
		else	// colour blend
		{
			// sdf doesn't change! we only update colors
			float2 tmp = add_smoothMin2(sdf, curSdf, SDFBlendFactor[i]);
			blendT = tmp.y;
		}
	}

	return sdf.x;
}

// Version that calculates material properties
float sceneSdf(float3 pos, float3 normal, out float4 outColor, out float outSmoothness, out float outMetallic, out float4 outEmissionColor)
{
	float3 posTransformed;
	float2 sdf = float2(FLT_MAX, 1.0);

	outColor = float4(0, 0, 0, 0);
	outSmoothness = 0;
	outMetallic = 0;
	outEmissionColor = float4(0, 0, 0, 0);

	float4 texturedColor = float4(0, 0, 0, 0);

	for (int i = 0; i < SDFCount; i++)
	{
		float size = SDFData[i].x;
		int blendOp = SDFBlendOperation[i];
		int sdfType = SDFType[i];

		float4x4 transform = SDFTransformMatrices[i];
		posTransformed = mul(transform, float4(pos, 1.0)).xyz;

		float roundness = SDFData[i].w;	// .w is always roundness

		float curSdf = 0.0f;
		float blendT = 0.0f;

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

		if (blendOp == 0)	// add
		{
			sdf = add_smoothMin2(sdf, curSdf, SDFBlendFactor[i]);
			blendT = sdf.y;
		}
		else if (blendOp == 1)	// subtract
		{
			sdf = subtract(sdf, curSdf, SDFBlendFactor[i]);
			blendT = sdf.y;
		}
		else if (blendOp == 2)	// intersect
		{
			sdf = intersect(sdf, curSdf);
			blendT = sdf.y;
		}
		else	// colour blend
		{
			// sdf doesn't change! we only update colors
			float2 tmp = add_smoothMin2(sdf, curSdf, SDFBlendFactor[i]);
			blendT = tmp.y;
		}

		GetTexture(posTransformed, mul(transform, float4(normal, 1)).xyz, i, texturedColor);
		outColor = lerp(outColor, texturedColor, abs(blendT));
		outSmoothness = lerp(outSmoothness, SDFSmoothness[i], abs(blendT));
		outMetallic = lerp(outMetallic, SDFMetallic[i], abs(blendT));
		outEmissionColor = lerp(outEmissionColor, SDFEmissionColors[i], abs(blendT));
	}

	return sdf.x;
}

float3 CalculateNormal(float3 pos)
{
	return normalize(float3(sceneSdf(pos + EPSILON.yxx) - sceneSdf(pos - EPSILON.yxx),
							sceneSdf(pos + EPSILON.xyx) - sceneSdf(pos - EPSILON.xyx),
							sceneSdf(pos + EPSILON.xxy) - sceneSdf(pos - EPSILON.xxy)));
}

void Raymarch_float(float3 rayOriginObjectSpace, float3 rayDirectionObjectSpace, out float3 outPosition, out float4 outColor, out float3 objectSpaceNormal, out float outSmoothness, out float outMetallic, out float4 outEmissionColor)
{
	float dist = MIN_DIST;

	for (int i = 0; i < MAX_ITERS; i++)
	{
		float3 p = rayOriginObjectSpace + rayDirectionObjectSpace * dist;

		float m = sceneSdf(p);

		if (m <= MIN_DIST)
		{
			// hit the sphere
			outPosition = p;
			objectSpaceNormal = CalculateNormal(p);

			// now get material properties
			sceneSdf(p, objectSpaceNormal, outColor, outSmoothness, outMetallic, outEmissionColor);

			break;
		}

#if RAYMARCH_CONSTANT_STEPS
		dist += MIN_DIST;
#elif RAYMARCH_SPHERE_TRACE
		dist += clamp(m, MIN_DIST, m);
#endif
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