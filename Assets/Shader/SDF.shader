Shader "Custom/SDF"
{
    Properties
    {
        _MainTex ("Albedo", 2D) = "white" {}
        _NormTex ("Normal", 2D) = "white" {}
        _StartColor ("Start Color", Color) = (1,1,1,1)
        _EndColor   ("End Color", Color) = (1,1,1,1)
        //_ParentWorldToLocal ("P2L", Matrix) = {}
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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _NormTex;
            float4 _MainTex_ST;
            float4 _StartColor;
            float4 _EndColor;
            float4x4 _ParentWorldToLocal;
            v2f vert (appdata v)
            {
                v2f o;
                o.worldPos = mul(_ParentWorldToLocal, mul(unity_ObjectToWorld, v.vertex)).xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 p = float3(i.worldPos.x, 0.f, i.worldPos.z);
                float distance = length(p - float3(0.f, 0.f, 0.f));
                float max_dist = 2.f;
                float r = distance / max_dist;

                float3 color = lerp(_StartColor.rgb, _EndColor.rgb, r);

                // sample the texture
                //fixed4 col = fixed4(color, 1);
                fixed4 col = tex2D(_MainTex, float2(0.5f, r * 30.f));//fixed4(color, 1);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
