#ifndef TEXTURINGSHADERINCLUDE
#define TEXTURINGSHADERINCLUDE

Texture2DArray<float4> TextureArraySide;
Texture2DArray<float4> TextureArrayTop;
Texture2DArray<float4> TextureArrayFront;

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

#endif