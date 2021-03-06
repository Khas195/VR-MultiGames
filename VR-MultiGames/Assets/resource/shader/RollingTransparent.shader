﻿Shader "Custom/RollingTransparent"
{
	Properties
	{
		_Color ("Transparent", color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_ScrollXSpeed("Scroll X Speed", Range(-10,10)) = 2
		_ScrollYSpeed("Scroll Y Speed", Range(-10,10)) = 2
	}
	SubShader
	{
		Tags { 
		"RenderType"="Transparent"
		 "Queue" = "Transparent"
		 "Ignore Projector" = "True"
		  }
		LOD 100

		Blend One One
		ZWrite Off
		Cull Off


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
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _ScrollXSpeed;
			fixed _ScrollYSpeed;
			fixed4 _Color;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed2 scrolledUV = i.uv;

				fixed xScrollValue = _ScrollXSpeed * _Time;
				fixed yScrollValue = _ScrollYSpeed * _Time;
				scrolledUV += fixed2(xScrollValue, yScrollValue);

				half4 col = tex2D(_MainTex,scrolledUV);

				col *= _Color;

				return col;
			}
			ENDCG
		}
	}
}
