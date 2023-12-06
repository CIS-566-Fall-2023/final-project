#ifndef RAYMARCHSHADERINCLUDE
#define RAYMARCHSHADERINCLUDE

#include "Common.hlsl"
#include "Texturing.hlsl"

#define RAYMARCH_CONSTANT_STEPS 1
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

bool intersectBox(float3 rayOrigin, float3 rayDirection, out float tNear, out float tFar) {
	float3 rinvDir = 1.0 / rayDirection;
	// for now we're assuming unit cube that is NOT moved!
	float3 tbot = rinvDir * (float3(-0.5, -0.5, -0.5) - rayOrigin);
	float3 ttop = rinvDir * (float3(0.5, 0.5, 0.5) - rayOrigin);
	float3 tmin = min(ttop, tbot);
	float3 tmax = max(ttop, tbot);
	float2 t = max(tmin.xx, tmin.yz);
	float t0 = max(t.x, t.y);
	t = min(tmax.xx, tmax.yz);
	float t1 = min(t.x, t.y);
	tNear = t0;
	tFar = t1;
	return t1 > max(t0, 0.0);
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
float sdfCompoundObjectParent(float3 pos)
{
	return 0.0f;
}

float sdfBox(float3 pos, float3 size)
{
	//if (pos.y > 0.1)
	//{
	//	pos.y += sin(10 * pos.x) * sin(20 * pos.z) * 0.05;
	//}
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

float sdfCappedCone(float3 pos, float height, float r1, float r2)
{
	float2 q = float2(length(pos.xz), pos.y);
	float2 k1 = float2(r2, height);
	float2 k2 = float2(r2 - r1, 2.0 * height);
	float2 ca = float2(q.x - min(q.x, (q.y < 0.0) ? r1 : r2), abs(q.y) - height);
	float2 cb = q - k1 + k2 * clamp(dot(k1 - q, k2) / dot(k2, k2), 0.0, 1.0);
	float s = (cb.x < 0.0 && ca.y < 0.0) ? -1.0 : 1.0;
	return s * sqrt(min(dot(ca,ca), dot(cb,cb)));
}

float sdfRoundness(float sdf, float roundness)
{
	return sdf - roundness;
}

float sdfElement(float3 pos, int index, int lastIndex)
{
	float3 posTransformed;
	float2 sdf = float2(MAX_DIST, 1.0);

	for (int i = index; i < lastIndex; i++)
	{
		float size = SDFData[i].x;
		int blendOp = i == index? 0 : SDFBlendOperation[i];	// the first individual element should always ADD 
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
			curSdf = sdfCappedCone(posTransformed, size, SDFData[i].y, SDFData[i].z);
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

// Version that only calculates sdf minDist
float sceneSdf(float3 pos)
{
	float2 sdf = float2(FLT_MAX, 1.0);

	int i = 0;
	int sdfsDone = 0;
	while (sdfsDone < SDFCountCompounded)
	{
		int sdfType = SDFType[i];
		int elementIndex = i;
		int blendOp = SDFBlendOperation[i];
		int elemBlendOp = 0;	// add for individual element blends
		if (sdfType == -1)	// compound sdf
		{
			elementIndex++;	// this is the parent. ignore and go to children
		}

		float curSdf = sdfElement(pos, elementIndex, OffsetsToNextSdf[sdfsDone]);
		float blendT = 0.0f;

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

		i = OffsetsToNextSdf[sdfsDone];
		sdfsDone++;
	}

	return sdf.x;
}

float sdfElement(float3 pos, int index, int lastIndex, float3 normal, out float4 outColor, out float outSmoothness, out float outMetallic, out float4 outEmissionColor)
{
	float3 posTransformed;
	float2 sdf = float2(FLT_MAX, 1.0);

	float4 texturedColor = float4(0, 0, 0, 0);
	bool isFirst;

	for (int i = index; i < lastIndex; i++)
	{
		isFirst = i == index;
		float size = SDFData[i].x;
		int blendOp = isFirst ? 0 : SDFBlendOperation[i];	// the first individual element should always ADD 
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
			curSdf = sdfCappedCone(posTransformed, size, SDFData[i].y, SDFData[i].z);
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
// Version that calculates material properties
float sceneSdf(float3 pos, float3 normal, out float4 outColor, out float outSmoothness, out float outMetallic, out float4 outEmissionColor)
{
	float3 posTransformed;
	float2 sdf = float2(FLT_MAX, 1.0);

	outColor = float4(0, 0, 0, 0);
	outSmoothness = 0;
	outMetallic = 0;
	outEmissionColor = float4(0, 0, 0, 0);

	float4 curOutColor = float4(0, 0, 0, 0);
	float4 texturedColor = float4(0, 0, 0, 0);
	float4 curTexturedColor = float4(0, 0, 0, 0);
	float curSmoothness = 0;
	float curMetallic = 0;
	float4 curEmissionColor = float4(0, 0, 0, 0);

	int i = 0;
	int sdfsDone = 0;

	while (sdfsDone < SDFCountCompounded)
	{
		float blendT = 0.0f;

		int sdfType = SDFType[i];
		int elementIndex = i;
		if (sdfType == -1)	// compound sdf
		{
			elementIndex++;	// this is the parent. ignore and go to children
		}

		float4 discarding;
		float discardingg;
		curOutColor = float4(0, 0, 0, 0);
		curTexturedColor = float4(0, 0, 0, 0);

		float curSdf = sdfElement(pos, elementIndex, OffsetsToNextSdf[sdfsDone], normal, curOutColor, curSmoothness, curMetallic, curEmissionColor);
		int blendOp = SDFBlendOperation[i];

		float4x4 transform = SDFTransformMatrices[i];
		posTransformed = mul(transform, float4(pos, 1.0)).xyz;

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

		if (sdfType == -1)
		{
			// compound sdf, apply its texture too
			GetTexture(posTransformed, mul(transform, float4(normal, 1)).xyz, i, curTexturedColor);
			curOutColor *= curTexturedColor;
			curSmoothness *= SDFSmoothness[i];
			curMetallic *= SDFMetallic[i];
			curEmissionColor *= SDFEmissionColors[i];
		}

		// blend each individual (or set of compound) element material
		outColor = lerp(outColor, curOutColor, abs(blendT));
		outSmoothness = lerp(outSmoothness, curSmoothness, abs(blendT));
		outMetallic = lerp(outMetallic, curMetallic, abs(blendT));
		outEmissionColor = lerp(outEmissionColor, curEmissionColor, abs(blendT));

		i = OffsetsToNextSdf[sdfsDone];
		sdfsDone++;
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
	float tNear, tFar;
	if (!intersectBox(rayOriginObjectSpace, rayDirectionObjectSpace, tNear, tFar))
	{
		// don't raymarch if ray doesn't intersect the AABB of the object
		outPosition = rayOriginObjectSpace + rayDirectionObjectSpace;
		outColor = float4(0, 0, 0, 0);
		objectSpaceNormal = float3(0, 0, 0);
		outSmoothness = 0;
		outMetallic = 0;
		outEmissionColor = float4(0, 0, 0, 0);
		return;
	}

	tFar = min(tFar, MAX_DIST);
	float dist = max(MIN_DIST, tNear);

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
		if (dist >= tFar)
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