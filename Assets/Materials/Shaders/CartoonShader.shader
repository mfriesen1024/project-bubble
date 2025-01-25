Shader "Custom/KennyStyleCartoon"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (1,1,1,1)
        _Ramp ("Ramp Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0.2,0.2,0.2,1)
        _OutlineWidth ("Outline Width", Range(0.0, 0.1)) = 0.015
        _ShadowColor ("Shadow Color", Color) = (0.5,0.5,0.7,1)
        _Specular ("Specular Size", Range(0,1)) = 0.5
        _RimPower ("Rim Power", Range(0,5)) = 2.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" }
        LOD 200

        // Outline Pass
        Pass
        {
            Cull Front
            ZWrite On
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            v2f vert (appdata v)
            {
                v2f o;
                float3 normal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                float2 offset = TransformViewToProjection(normal.xy);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.pos.xy += offset * _OutlineWidth;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }

        // Base Color Pass
        Pass
        {
            Cull Back
            ZWrite On
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
                SHADOW_COORDS(4)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Ramp;
            float4 _Color;
            float4 _ShadowColor;
            float _Specular;
            float _RimPower;
            float4 _OutlineColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = WorldSpaceViewDir(v.vertex);
                TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Basic vectors
                float3 normal = normalize(i.worldNormal);
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 viewDir = normalize(i.viewDir);
                
                float NdotL = dot(normal, lightDir);
                float2 rampUV = float2(NdotL * 0.5 + 0.5, 0.5);
                float4 ramp = tex2D(_Ramp, rampUV);
                
                float shadow = SHADOW_ATTENUATION(i);
                
                // Soft shadow blending
                float3 shadowColor = lerp(_ShadowColor.rgb, float3(1,1,1), shadow);
                
                float3 halfVector = normalize(lightDir + viewDir);
                float specular = pow(max(dot(normal, halfVector), 0), _Specular * 128);
                specular = step(0.9, specular);
                
                // Rim lighting
                float rim = 1 - saturate(dot(normal, viewDir));
                rim = pow(rim, _RimPower);
                
                float4 texColor = tex2D(_MainTex, i.uv) * _Color;
                float3 finalColor = texColor.rgb * ramp.rgb * shadowColor;
                finalColor += specular * _LightColor0.rgb;
                finalColor += rim * _ShadowColor.a * _ShadowColor.rgb;
                
                return float4(finalColor, texColor.a);
            }
            ENDCG
        }

        // Shadow pass
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
    FallBack "Diffuse"
}