#ifndef TEXTURINGSHADERINCLUDE
#define TEXTURINGSHADERINCLUDE

#include "Common.hlsl"
#include "NoiseFunctions.hlsl"

Texture2DArray<float4> TextureArraySide;
Texture2DArray<float4> TextureArrayTop;
Texture2DArray<float4> TextureArrayFront;

/* ====================================================
* ================ SAMPLED TEXTURES ===================
*==================================================== */

void GetTriplanarTexture(float sdfObjectIndex, float3 position, float3 normal, float blendStrength, UnitySamplerState samplerstate, out float3 outColor)
{
	// assume position is always between -0.5 and 0.5!!!
	position = position + 0.5;	// normalize between 0 and 1

	float2 uv_xy = position.xy;
	float2 uv_yz = position.yz;
	float2 uv_xz = position.xz;

	normal = pow(abs(normal), blendStrength);	// abs because same left and right, pow for making the sides separated
	normal /= (normal.x + normal.y + normal.z);	// normalzing after pow operation


	// Other way of sampling:
	//		SAMPLE_TEXTURE2D_ARRAY(TextureArraySide, samplerstate.samplerstate, uv_xy, sdfObjectIndex) * normal.z;
	// look into this script: https://github.com/Unity-Technologies/Graphics/blob/master/Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl

	outColor = TextureArrayFront.SampleLevel(samplerstate.samplerstate, float3(uv_xy, sdfObjectIndex), 0) * normal.z
			 + TextureArraySide.SampleLevel(samplerstate.samplerstate, float3(uv_yz, sdfObjectIndex), 0) * normal.x
			 + TextureArrayTop.SampleLevel(samplerstate.samplerstate, float3(uv_xz, sdfObjectIndex), 0) * normal.y;

	//outColor = normal;
}

/* ========================================================
* ============= 2D UV PROCEDURAL TEXTURES =================
*========================================================== */

float GetStripes(float3 pos, float2 values)
{
	pos.y = cos(values.y) * pos.x + sin(values.y) * pos.y;
	return smoothstep(0.5, values.x, sin(pos.y));
}

float GetDots(float2 pos, float2 smoothstepEdges)
{
	return smoothstep(smoothstepEdges.x, smoothstepEdges.y, sin(pos.x) * cos(pos.y));
}

float GetDiamonds(float2 pos, float2 smoothstepEdges)
{
	return smoothstep(smoothstepEdges.x, smoothstepEdges.y, sin(pos.x) + cos(pos.y));
}

float GetWaves(float2 pos, float2 values)
{
	return smoothstep(values.x - 0.1, values.x, sin(pos.x + sin(pos.y * values.y)));
}

float GetPattern1(float2 pos, float2 values)
{
	return sin(pos.x * pos.y);
	return smoothstep(0.5, values.x, sin(pos.x * pos.y));
}

float GetTriplanarTexture(float3 position, float3 normal, float4 curSDFTextureData, float SDFTextureType)
{
	position *= curSDFTextureData.x;	// texture scale

	normal = pow(abs(normal), curSDFTextureData.w);	// abs because same left and right, pow for making the sides separated
	normal /= (normal.x + normal.y + normal.z);	// normalzing after pow operation

	float texX = 0;
	float texY = 0;
	float texZ = 0;

	if (SDFTextureType == 1)	// stripes horizontal
	{
		texX = GetStripes(position, curSDFTextureData.yz);
		texY = GetStripes(position, curSDFTextureData.yz);
		texZ = GetStripes(position, curSDFTextureData.yz);
	}
	else if (SDFTextureType == 2)	// dots
	{
		texX = GetDots(position.yz, curSDFTextureData.yz);
		texY = GetDots(position.xz, curSDFTextureData.yz);
		texZ = GetDots(position.xy, curSDFTextureData.yz);
	}
	else if (SDFTextureType == 3)	// diamonds
	{
		texX = GetDiamonds(position.yz, curSDFTextureData.yz);
		texY = GetDiamonds(position.xz, curSDFTextureData.yz);
		texZ = GetDiamonds(position.xy, curSDFTextureData.yz);
	}
	else if (SDFTextureType == 4)	// waves
	{
		texX = GetWaves(position.yz, curSDFTextureData.yz);
		texY = GetWaves(position.xz, curSDFTextureData.yz);
		texZ = GetWaves(position.yx, curSDFTextureData.yz);
	}
	else if (SDFTextureType == 5)	// waves
	{
		texX = GetPattern1(position.yz, curSDFTextureData.yz);
		texY = GetPattern1(position.xz, curSDFTextureData.yz);
		texZ = GetPattern1(position.yx, curSDFTextureData.yz);
	}

	float tex = texX * normal.x + texY * normal.y + texZ * normal.z;
	return tex;
}

/* ========================================================
* ============= 3D UV PROCEDURAL TEXTURES =================
*========================================================== */

float GetWorley(float3 pos, float2 values)
{
	return smoothstep(values.x, values.y, WorleyNoise3D(pos));
}

float GetWorleyCells(float3 pos, float2 values)
{
	float worl = WorleyNoise3D(pos);
	worl = worl * worl * worl * worl * worl;
	return worl;
	//return step(values.x, worl);
}

float GetFBM(float3 pos, float2 values)
{
	float fbmVal = fbm(pos, values.x, values.y);
	return fbmVal;
}

float GetPerlin(float3 pos, float2 values)
{
	float perl = perlinNoise3D(pos);
	perl = smoothstep(0.5, values.x, perl);
	return perl;
}

float Get3DTexture(float3 pos, float3 normal, float4 curSDFTextureData, float SDFTextureType)
{
	pos *= curSDFTextureData.x;	// texture scale

	float tex = 0;
	if (SDFTextureType == 6)	// worley
	{
		tex = GetWorley(pos, curSDFTextureData.yz);
	}
	else if (SDFTextureType == 7)	// worley cells
	{
		tex = GetWorleyCells(pos, curSDFTextureData.yz);
	}
	else if (SDFTextureType == 8)	// FBM
	{
		tex = GetFBM(pos, curSDFTextureData.yz);
	}
	else if (SDFTextureType == 9)	// Perlin
	{
		tex = GetPerlin(pos, curSDFTextureData.yz);
	}
	return tex;
}

void GetTexture(float3 worldPosition, float3 worldNormal, int sdfObjectIndex, out float4 textureValue)
{
	float tex = 0.0f;
	float4 textureData = SDFTextureData[sdfObjectIndex];
	float type = SDFTextureType[sdfObjectIndex];

	if (type == 0)	// no texture
	{
		tex = 0;
	}
	else if (type >= 1 && type <= 5)	// triplanar textures
	{
		tex = GetTriplanarTexture(worldPosition, worldNormal, textureData, type);
		//tex = smoothstep(0.5,0.7, sin(position.x / 0.01f)) * smoothstep(0.5,0.7,cos(position.y / 0.01f)) * smoothstep(0.5,0.7,sin(position.z / 0.01f));
	}
	else if (type >= 6)	// 3D position based texture
	{
		tex = Get3DTexture(worldPosition, worldNormal, textureData, type);
	}

	textureValue = lerp(SDFPrimaryColors[sdfObjectIndex], SDFSecondaryColors[sdfObjectIndex], tex);
}

#endif