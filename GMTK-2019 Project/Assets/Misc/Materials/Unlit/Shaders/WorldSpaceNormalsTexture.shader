// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/WorldSpaceNormalsTexture"
{
Properties {
     _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	 _Color ("Main Color", Color) = (1,1,1,1)
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
             #pragma multi_compile_fog
             
             #include "UnityCG.cginc"
 
             struct appdata_t {
                 float4 vertex : POSITION;
                 float2 texcoord : TEXCOORD0;
				 float3 normal : NORMAL;
             };
 
             struct v2f {
                 float4 vertex : SV_POSITION;
                 half2 texcoord : TEXCOORD0;
				 half3 worldNormal: NORMAL;

		//half3 worldNormal : TEXCOORD0;
		//float4 pos : SV_POSITION;
                 UNITY_FOG_COORDS(1)
             };
 
             sampler2D _MainTex;
             float4 _MainTex_ST;
             
             v2f vert (appdata_t v)
             {
                 v2f o;
                 o.vertex = UnityObjectToClipPos(v.vertex);
                 o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				 //o.pos = UnityObjectToClipPos(vertex);
                // UnityCG.cginc file contains function to transform
                // normal from object to world space, use that
                o.worldNormal = UnityObjectToWorldNormal(v.normal);


                 UNITY_TRANSFER_FOG(o,o.vertex);
                 return o;
             }
             
			 // color from the material
            fixed4 _Color;

             fixed4 frag (v2f i) : SV_Target
             {
			 
                 /*fixed4 col = tex2D(_MainTex, i.texcoord);
                 UNITY_APPLY_FOG(i.fogCoord, col);
                 return col;*/
				 

				 fixed4 c = 0;
				 c = tex2D(_MainTex, i.texcoord);


                // normal is a 3D vector with xyz components; in -1..1
                // range. To display it as color, bring the range into 0..1
                // and put into red, green, blue components
                c.rgb *= i.worldNormal*0.5+0.5;

				
				c.r = c.r;
				c.g = c.r;
				c.b = c.r;
				
				c.rgba *= _Color;
				//c.rgb = ( (i.worldNormal*0.5+0.5) + _Color) * 0.5;
				//c.rgb = half4(((i.worldNormal*0.5+0.5).rgb * 0.5 +_Color.rgb * 1), 1 + 1);

				//c.a = 0.5;
				//c.rgb = c.rgb * Color.rgb;
                return c;
             }
         ENDCG
     }
 }
 
}