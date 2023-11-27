void GetMainLight_float(float3 WorldPos, out float3 Color, out float3 Direction, out float DistanceAtten, out float ShadowAtten) {
#ifdef SHADERGRAPH_PREVIEW
    Direction = normalize(float3(0.5, 0.5, 0));
    Color = 1;
    DistanceAtten = 1;
    ShadowAtten = 1;
#else
    #if SHADOWS_SCREEN
        float4 clipPos = TransformWorldToClip(WorldPos);
        float4 shadowCoord = ComputeScreenPos(clipPos);
    #else
        float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    #endif

    Light mainLight = GetMainLight(shadowCoord);
    Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
    ShadowAtten = mainLight.shadowAttenuation;
#endif
}

void ChooseColor_float(float3 Highlight, float3 Shadow, float Diffuse, float Threshold, out float3 OUT)
{
    if (Diffuse < Threshold)
    {
        OUT = Shadow;
    }
    else
    {
        OUT = Highlight;
    }
}

void ChooseColorSand_float(bool Sand, float Rand, 
                           float3 Sand1, float3 Sand2, float3 Sand3,
                           float3 Highlight, float3 Shadow, 
                           float Diffuse, float Threshold, out float3 OUT)
{
    if(Sand)
    {
        float3 col = Sand1;
        if (Rand > 0.33)
        {
            col = Sand2;
        }
        if (Rand > 0.66)
        {
            col = Sand3;
        }
        
        if (Diffuse < Threshold)
        {
            OUT = col * 0.5;
        }
        else
        {
            OUT = col;
        }
    }
    else
    {
        if (Diffuse < Threshold)
        {
            OUT = Shadow;
        }
        else
        {
            OUT = Highlight;
        }
    }   
}