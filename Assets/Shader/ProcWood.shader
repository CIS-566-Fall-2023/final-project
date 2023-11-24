// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/ProcWood"
{
    Properties
    {
        _MaxHeight("Max Height", Float) = 1
        _AnimationTime("Animation Time", Float) = 1
        _MinRadius("Min Radius", Float) = 1
        _MaxRadius("Max Radius", Float) = 1
        _KnotCount("Knot count", Float) = 1
        _ColorMap("Color Map", 2D) = "white" {}
        _HeightMap("Height Map", 2D) = "white" {}
        _OrientationMap("Orientation Map", 2D) = "white" {}
        _StateMap("State Map", 2D) = "white" {}
        _PithRadiusMap("Pith Radius Map", 2D) = "white" {}
        //_MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #define MaxKnotCount 20
            #define Pi 3.1415926535897932
            #define RangeK float2(0.8, 1.0)
            #define RangeD float2(0.1, 0.2)
            #define GET_TEXELSIZE(tex) tex##_TexelSize

            float4 cubic(float x) {
                float x2 = x * x;
                float x3 = x2 * x;

                return float4(
                    -x3 + 3.0 * x2 - 3.0 * x + 1.0,
                    3.0 * x3 - 6.0 * x2 + 4.0,
                    -3.0 * x3 + 3.0 * x2 + 3.0 * x + 1.0,
                    x3
                ) / 6.0;
            }

            //float2 getTextureSize(sampler2D tex) {
            //    return UNITY_TEXTURE_COORDS_SIZE(tex);
            //}

            float4 sampleTextureBicubic(sampler2D tex, float4 texTexelSize, float2 uv) {
                float2 textureSize = texTexelSize.zw;
                float2 texelSize = texTexelSize.xy;

                uv = uv * textureSize - 0.5;

                float2 fuv = frac(uv);
                uv -= fuv;

                float4 xcubic = cubic(fuv.x);
                float4 ycubic = cubic(fuv.y);

                float4 c = uv.xxyy + float2(-0.5, 1.5).xyxy;

                float4 s = float4(xcubic.xz + xcubic.yw, ycubic.xz + ycubic.yw);
                float4 offset = c + float4(xcubic.yw, ycubic.yw) / s;

                offset *= texelSize.xxyy;

                float4 sample0 = tex2D(tex, offset.xz);
                float4 sample1 = tex2D(tex, offset.yz);
                float4 sample2 = tex2D(tex, offset.xw);
                float4 sample3 = tex2D(tex, offset.yw);

                float sx = s.x / (s.x + s.y);
                float sy = s.z / (s.z + s.w);

                return lerp(lerp(sample3, sample2, sx), lerp(sample1, sample0, sx), sy);
            }

            void SampleTexture2DBicubic_float(sampler2D tex, float4 texTexelSize, float2 uv, out float4 color) {
                color = sampleTextureBicubic(tex, texTexelSize, uv);
            }

            float mod(float x, float m) {
                return frac(x / m) * m;
            }

            float4 mod(float4 x, float4 m) {
                return frac(x / m) * m;
            }

            float4 mod289(float4 x) {
                return frac(x / 289.0) * 289.0;
            }

            float4 permute(float4 x) {
                return mod289((x * 34.0 + 1.0) * x);
            }

            float4 fastInvSqrt(float4 x) {
                return 1.79284291400159 - 0.85373472095314 * x;
            }

            float2 fade(float2 t) {
                return t * t * t * (t * (t * 6.0 - 15.0) + 10.0);
            }

            float PerlinNoise(float2 p, float2 freq, float seed) {
                float4 ip = floor(p.xyxy) + float4(0, 0, 1, 1);
                float4 fp = frac(p.xyxy) - float4(0, 0, 1, 1);

                ip = mod(ip, freq.xyxy);
                ip = mod289(ip);

                float4 ix = ip.xzxz + seed;
                float4 iy = ip.yyww + seed;
                float4 fx = fp.xzxz;
                float4 fy = fp.yyww;

                float4 i = permute(permute(ix) + iy);

                float4 gx = frac(i / 41.0) * 2.0 - 1.0;
                float4 gy = abs(gx) - 0.5;
                gx -= floor(gx + 0.5);

                float2 g00 = float2(gx.x, gy.x);
                float2 g10 = float2(gx.y, gy.y);
                float2 g01 = float2(gx.z, gy.z);
                float2 g11 = float2(gx.w, gy.w);

                float4 norm = fastInvSqrt(float4(dot(g00, g00), dot(g01, g01), dot(g10, g10), dot(g11, g11)));
                g00 *= norm.x;
                g01 *= norm.y;
                g10 *= norm.z;
                g11 *= norm.w;

                float n00 = dot(g00, float2(fx.x, fy.x));
                float n10 = dot(g10, float2(fx.y, fy.y));
                float n01 = dot(g01, float2(fx.z, fy.z));
                float n11 = dot(g11, float2(fx.w, fy.w));

                float2 fadeVal = fade(fp.xy);
                float2 nx = lerp(float2(n00, n01), float2(n10, n11), fadeVal.x);
                float nxy = lerp(nx.x, nx.y, fadeVal.y);
                return 2.3 * nxy;
            }

            void PerlinNoise_float(float2 p, float2 freq, float seed, out float noise) {
                noise = PerlinNoise(p, freq, seed);
            }

            float combinedNoises(float2 scale, float3 v0, float3 v1, float3 v2, float seed) {
                float3 noise = float3(
                    PerlinNoise(v0.xy * scale, v0.xy, seed),
                    PerlinNoise(v1.xy * scale, v1.xy, seed),
                    PerlinNoise(v2.xy * scale, v2.xy, seed)
                );
                return noise * float3(v0.z, v1.z, v2.z);
            }

            float powerSmoothMin(float a, float b, float k) {
                a = pow(a, k);
                b = pow(b, k);
                return pow(a * b / (a + b), 1.0 / k);
            }

            float powerSmoothMin(float a[MaxKnotCount], float k) {
                float product = 1.0;

                for (uint i = 0; i < MaxKnotCount; i++) {
                    a[i] = pow(a[i], k);
                    product *= a[i];
                }
                float sum = 0.0;

                for (uint i = 0; i < MaxKnotCount; i++) {
                    if (a[i] != 0) {
                        sum += product / a[i];
                    }
                }
                return pow(product / sum, 1.0 / k);
            }

            float vectorSum(float v[MaxKnotCount]) {
                float sum = 0.0;
                for (uint i = 0; i < MaxKnotCount; i++) {
                    sum += v[i];
                }
                return sum;
            }

            float vectorMin(float v[MaxKnotCount]) {
                float minVal = 1e8;
                for (uint i = 0; i < MaxKnotCount; i++) {
                    minVal = min(minVal, v[i]);
                }
                return minVal;
            }

            void MapLocalPositionTo3DUV_float(float3 uv, float time, float zRatio, out float3 mapped) {
                mapped.x = uv.x;
                mapped.y = uv.y;
                mapped.z = uv.z/ zRatio;
                
                //mapped.y = uv.y + 0.18;
                //mapped.z = (uv.z + mod(0.25 * time, zRatio)) / zRatio;
            }

            void StemGeometry_float(
                sampler2D pithRadiusMap, float4 pithMapTexelSize, float3 uv, float zRatio, float minRadius, float maxRadius,
                out float3 position, out float localMaxRadius, out float horizontalDistance, out float timeValue
            ) {
                float3 pithRadius = sampleTextureBicubic(pithRadiusMap,pithMapTexelSize, float2(0.5, uv.z)).rgb;
                uv.xy -= pithRadius.xy - 0.5;

                float3 pseudoClosestPoint = float3(0, 0, uv.z * zRatio);
                position = float3(uv.xy, uv.z * zRatio);
                float angle = atan2(uv.x, uv.y);
                float theta = clamp(angle / (2.0 * Pi) + 0.5, 0., 1.f);
                pithRadius = sampleTextureBicubic(pithRadiusMap, pithMapTexelSize, float2(theta, uv.z)).rgb;
                //pithRadius = tex2D(pithRadiusMap, float2(theta, uv.z)).rgb;

                localMaxRadius = pithRadius.b * (maxRadius / minRadius - 1.0) + 1.0;
                horizontalDistance = distance(pseudoClosestPoint, position);
                timeValue = horizontalDistance / localMaxRadius;
            }

            void DistanceRange_float(float horizontalDistance, out float2 distanceRange) {
                distanceRange = RangeD + RangeK * horizontalDistance;
            }

            void WoodTexture_float(
                sampler2D colorMap, sampler2D heightMap, sampler2D orientationMap, sampler2D stateMap, 
                uint numKnots, float3 position, float localMaxRadius, float horizontalDistance, float timeValue, float zRatio, float2 distanceRange,
                out float3 color
            ) {
                float skeletonDistances[MaxKnotCount];
                float timeValues[MaxKnotCount];
                float deathTimes[MaxKnotCount];
                float orientations[MaxKnotCount];
                uint indices[MaxKnotCount];

                for (uint i = 0; i < MaxKnotCount; i++) {
                    skeletonDistances[i] = 9.0;
                    timeValues[i] = 9.0;
                    deathTimes[i] = 9.0;
                    orientations[i] = 0.0;
                    indices[i] = 0;
                }

                uint count = 0;
                [unroll(4)]
                for (uint i = 0; i < numKnots; i++) {
                    float2 uv = float2(horizontalDistance, (float(i) + 0.5) / numKnots);
                    //float3 height = sampleTextureBicubic(heightMap, uv).rgb;
                    //float3 orient = sampleTextureBicubic(orientationMap, uv).rgb;
                    float3 height = tex2D(heightMap, uv).rgb;
                    float3 orient = tex2D(orientationMap, uv).rgb;
                    float3 state = tex2D(stateMap, uv).rgb;
                    float knotRadius = state.b;

                    float theta = orient.r * 2.0 * Pi + (orient.b - orient.g) * 0.5 * Pi;

                    float3 branch = float3(
                        horizontalDistance * cos(theta),
                        horizontalDistance * sin(theta),
                        height.r * zRatio + height.b - height.g
                    );

                    float distToBranch = distance(position, branch);

                    if (distToBranch < distanceRange.y) {
                        skeletonDistances[count] = distToBranch;
                        deathTimes[count] = state.g / localMaxRadius;

                        float3 betaVec = position - branch;
                        orientations[count] = mod(atan2(betaVec.z, betaVec.x * sin(-theta) + betaVec.y * cos(-theta)) + 2.0 * Pi, 2.0 * Pi);

                        float beta01 = orientations[count] / (2.0 * Pi);
                        float seed = float(i) / numKnots;
                        float noiseValue = combinedNoises(float2(beta01, horizontalDistance), float3(1.0, 1.0, 0.1), float3(2.5, 3.0, 0.1), float3(6.0, 7.0, 0.1), seed);
                        float branchRadius = knotRadius  +  0.5 * knotRadius *(noiseValue - sqrt(horizontalDistance));

                        timeValues[count] = min(distToBranch / branchRadius,9.0);
                        //count++;

                        if (distToBranch > distanceRange.x) {
                            float smooth = (distToBranch - distanceRange.x) / (distanceRange.y - distanceRange.x);
                            timeValues[count] = lerp(timeValues[count], 9.0, smooth);
                        }
                        indices[count++] = i;

                        if (count >= MaxKnotCount) {
                            break;
                        }
                    }
                }

                float combinedTime = 9.0;
                float minTime = 9.0;

                const float Ks = 1.5;
                const float Kb = 5.0;

                const float F1 = -1.5;
                const float F2 = 0.2;
                const float F3 = 8.0;
                const float F4 = 5.0;

                float deadColorFactor = 0;
                float deadOutlineFactor = 1.0;
                float deadOutlineThickness = 0.02;

                float delta[MaxKnotCount];

                for (uint i = 0; i < numKnots; i++) {
                    delta[i] = 0;

                    if (timeValues[i] < 9.0) {
                        float deltaTime = timeValue - timeValues[i];
                        float k = 0.5 * (Kb - Ks) * deltaTime / (0.3 + abs(deltaTime)) + 0.5 * (Kb + Ks);

                        combinedTime = powerSmoothMin(timeValue, timeValues[i], k);
                        delta[i] = combinedTime - min(timeValue, timeValues[i]);

                        if (combinedTime > deathTimes[i]) {
                            float timeAfterDeath = abs(timeValue - deathTimes[i]);

                            timeValues[i] += timeAfterDeath;
                            float combinedTime2 = powerSmoothMin(timeValue, timeValues[i], k + F4 * timeAfterDeath);
                            float deltaTime = combinedTime2 - min(timeValue, timeValues[i]);

                            float mappedTimeAfterDeath = F3 * timeAfterDeath - 1.0;
                            float fRange = F2 - F1;
                            float f = 1.0 - 0.5 * fRange * (mappedTimeAfterDeath / (0.3 + abs(mappedTimeAfterDeath))) + F1 + 0.5 * fRange;
                            delta[i] = f * deltaTime;
                        }

                        if (skeletonDistances[i] > distanceRange.x && skeletonDistances[i] < distanceRange.y) {
                            float smooth = (skeletonDistances[i] - distanceRange.x) / (distanceRange.y - distanceRange.x);
                            delta[i] -= delta[i] * smooth;
                        }

                        if (timeValues[i] < deathTimes[i] && timeValue > deathTimes[i]) {
                            deadColorFactor = timeValue - deathTimes[i];
                        }

                        if (timeValues[i] < deathTimes[i]) {
                            float beta01 = orientations[i] / (2.0 * Pi);
                            float seed = float(indices[i]) / numKnots;
                            float noiseValue = combinedNoises(float2(beta01, horizontalDistance), float3(2.0, 1.0, 0.005), float3(5.0, 2.0, 0.008), float3(23.0, 5.0, 0.01), seed);
                            deadOutlineThickness += noiseValue;

                            if (abs(deathTimes[i] - combinedTime) < deadOutlineThickness) {
                                deadOutlineFactor = 0.65;
                            }
                        }
                    }
                }
                minTime = powerSmoothMin(timeValues, 2.0);

                for (uint i = 0; i < numKnots; i++) {
                    float dt = max(timeValue - minTime, 0.0) / timeValue;
                    delta[i] -= delta[i] * smoothstep(0, 1, dt);
                }

                float sumDelta = vectorSum(delta);
                float minAllTime = min(timeValue, vectorMin(timeValues));
                combinedTime = minAllTime + sumDelta;

                float3 start = float3(1, 0, 0);
                float3 end = float3(0, 1, 0);

                color = lerp(start, end, combinedTime);

                color = tex2D(colorMap, float2(combinedTime, 0.5)).rgb;

                //color = tex2D(colorMap, float2(combinedTime, 0.5)).rgb;
                float3 knotColor = float3(0.2, 0.2, 0.15);
                float g = 1.0 / pow(clamp(1.2 * minTime - timeValue, 0.001, 1.0) + 1.0, 14);
                color -= g * knotColor;
                color -= g * clamp(3 * deadColorFactor, 0.0, 0.5) * knotColor;
                color *= deadOutlineFactor;
            }


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 wPos: TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _MaxHeight;
            float _AnimationTime;
            float _MinRadius;
            float _MaxRadius;
            float _KnotCount;
            sampler2D _ColorMap;
            float4 GET_TEXELSIZE(_ColorMap);
            sampler2D _HeightMap;
            float4 GET_TEXELSIZE(_HeightMap);
            sampler2D _OrientationMap;
            float4 GET_TEXELSIZE(_OrientationMap);
            sampler2D _StateMap;
            float4 GET_TEXELSIZE(_StateMap);
            sampler2D _PithRadiusMap;
            float4 GET_TEXELSIZE(_PithRadiusMap);
            float4x4 _ParentWorldToLocal;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.wPos = mul(_ParentWorldToLocal,mul(unity_ObjectToWorld, v.vertex)).xyz;
                //o.wPos = mul(_ParentWorldToLocal, v.vertex).xyz;
                o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float zRatio = _MaxHeight / _MinRadius;
                float3 geomUV; 
                MapLocalPositionTo3DUV_float(i.wPos, _AnimationTime, zRatio, geomUV);
                float3 outPos;
                float localMaxRadius;
                float horizontalDistance;
                float timeValue;
                StemGeometry_float(_PithRadiusMap, GET_TEXELSIZE(_PithRadiusMap), geomUV, zRatio, _MinRadius, _MaxRadius, outPos, localMaxRadius, horizontalDistance, timeValue);
                
                
                float2 distanceRange;
                DistanceRange_float(horizontalDistance,distanceRange);

                float3 col;
                WoodTexture_float(_ColorMap,_HeightMap,_OrientationMap,_StateMap,
                _KnotCount,outPos,localMaxRadius,horizontalDistance,timeValue,zRatio,distanceRange,
                col);

                fixed4 color = fixed4(col,1);//tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, color);
                return color;
            }
            ENDCG
        }
    }
}
