SAMPLER(sampler_point_clamp);

void GetDepth_float(float2 uv, out float Depth)
{
    Depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv);
}
static const float SQRTLOG2 = 0.83255461115;

void addFog_float(float Depth, float4 col, float4 _FogColor, float _FogDensity, float _FogOffset, out float4 fog) {

    float viewDistance = Depth;
    float fogFactor = float(_FogDensity / SQRTLOG2) * max(0.0, viewDistance - _FogOffset);
    fogFactor = exp2(-fogFactor * fogFactor);

    float4 fogOutput = lerp(_FogColor, col, saturate(fogFactor));

    fog = fogOutput;
}