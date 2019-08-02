//﻿

Shader "Unlit/UnlitGhost"
{
	Properties {

	 _Color ("Main Color", Color) = (1,1,1,1)
	 [PerRendererData]_AlphaOverride ("_AlphaOverride", Range(0,1)) = 1
 }
 
 SubShader {
     Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
     LOD 100
     
     //ZWrite Off
     Blend SrcAlpha OneMinusSrcAlpha 
     
     Pass {  
         CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             
             #include "UnityCG.cginc"
 
             struct appdata_t
			 {
                 float4 vertex : POSITION;
             };
 
             struct v2f
			 {
                 float4 vertex : SV_POSITION;
             };
 

             float4 _MainTex_ST;
             
             v2f vert (appdata_t v)
             {
                 v2f o;
                 o.vertex = UnityObjectToClipPos(v.vertex);
                 //o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                 return o;
             }
             
			 // color from the material
            fixed4 _Color;
			float _AlphaOverride;

             fixed4 frag (v2f i) : SV_Target
             {
				fixed4 c = 0;

				c.rgb = _Color;
				c.a = _AlphaOverride;

                return c;
             }
         ENDCG
     }
 }
 

}
