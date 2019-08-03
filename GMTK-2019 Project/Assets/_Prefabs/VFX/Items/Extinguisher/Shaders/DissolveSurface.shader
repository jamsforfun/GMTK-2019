﻿Shader "GMTK/Effect/Dissolvable Lit"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        
        [Space(5)]

        [NoScaleOffset] _Normal("Normal Map", 2D) = "bump" {}
        _NormalAmount("Normal Amount", Float) = 1.0


        [Space(5)]
        _Color("Color", Color) = (1,1,1,1)
        [HDR]_Emissive("Emissive Color", Color) = (0,0,0,0)

        [Space(10)]

        _Glossiness("Glossiness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.5   

        [Header(Dissolve)]
        _DissolveMask("Dissolve Mask", 2D) = "black" {}

        [Space(5)]

        _DissolveAmount("Dissolve Amount", Range(0,1)) = 0.0
        _AlphaCutoff("Alpha Cutoff", Range(0, 1)) = 0.0  
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Normal;
        sampler2D _DissolveMask;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_DissolveMask;
        };

        fixed _DissolveAmount;
        fixed _AlphaCutoff;

        float _NormalAmount;

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float4 _Emissive;

        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;

            fixed dissolve = tex2D(_DissolveMask, IN.uv_DissolveMask).r;
            dissolve -= _DissolveAmount;


            clip(dissolve - _AlphaCutoff);

            o.Smoothness = _Glossiness;

            float3 normal = UnpackNormal(tex2D(_Normal, IN.uv_MainTex));
            normal = normalize(float3(normal.x * _NormalAmount, normal.y * _NormalAmount, normal.z));

            o.Normal = normal;
            o.Alpha = c.a;
            o.Emission = _Emissive;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
