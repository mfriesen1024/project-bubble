Shader "Custom/KennyStyleCartoon_URP"
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
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "RenderPipeline"="UniversalPipeline" }
        LOD 200

        Pass
        {
            Name "Outline"
            Tags { "LightMode"="Always" }
            Cull Front
            ZWrite On
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            Varyings vert (Attributes v)
            {
                Varyings o;
                float3 normalWS = TransformObjectToWorldNormal(v.normalOS);
                float3 offset = normalWS * _OutlineWidth;
                float4 positionWS = TransformObjectToWorld(v.positionOS);
                o.positionCS = TransformWorldToHClip(positionWS + float4(offset, 0.0));
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }

        Pass
        {
            Name "MainPass"
            Tags { "LightMode"="UniversalForward" }
            Blend Alpha, OneMinusSrcAlpha
            Cull Back
            ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
                float3 worldPosWS : TEXCOORD3;
                LIGHT_COORDS(4)
            };

            sampler2D _MainTex;
            float4 _Color;
            sampler2D _Ramp;
            float4 _ShadowColor;
            float _Specular;
            float _RimPower;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                o.viewDirWS = GetCameraPositionWS() - TransformObjectToWorld(v.positionOS);
                o.worldPosWS = TransformObjectToWorld(v.positionOS);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                float3 normal = normalize(i.normalWS);
                float3 viewDir = normalize(i.viewDirWS);

                // Ramp shading
                float NdotL = saturate(dot(normal, normalize(GetMainLightDirection(i.worldPosWS))));
                float rampUV = NdotL * 0.5 + 0.5;
                float3 ramp = SAMPLE_TEXTURE2D(_Ramp, sampler_MainTex, float2(rampUV, 0.5)).rgb;

                // Shadow attenuation
                float shadowAtten = GetMainLightShadowAttenuation(i);
                float3 shadowColor = lerp(_ShadowColor.rgb, float3(1, 1, 1), shadowAtten);

                // Specular highlight
                float3 halfDir = normalize(normalize(GetMainLightDirection(i.worldPosWS)) + viewDir);
                float specular = pow(saturate(dot(normal, halfDir)), _Specular * 128.0);

                // Rim lighting
                float rim = pow(1.0 - saturate(dot(normal, viewDir)), _RimPower);

                // Final color
                float3 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv).rgb * _Color.rgb;
                float3 color = albedo * ramp * shadowColor;
                color += specular * GetMainLightColor().rgb;
                color += rim * _ShadowColor.a * _ShadowColor.rgb;

                return float4(color, _Color.a);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}
