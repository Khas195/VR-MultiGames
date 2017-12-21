// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Barrier"
{
	Properties
	{
		_Color ("Color", color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Thickness("Outline Thickness", float) = 2
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
		// Physically based Standard lighting model, and enable shadows on all light types
			 
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;			
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 viewDir : TEXCOORD1;		
				float3 normal : NORMAL;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float _Thickness;

			fixed _ScrollXSpeed;
			fixed _ScrollYSpeed;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed2 scrolledUV = i.uv;

				fixed xScrollValue = _ScrollXSpeed * _Time;
				fixed yScrollValue = _ScrollYSpeed * _Time;
				scrolledUV += fixed2(xScrollValue, yScrollValue);

				fixed4 mainTex = tex2D(_MainTex,scrolledUV);	
				float rim = 1 - abs(dot(i.normal, normalize(i.viewDir))) * 1.5;
				rim = saturate(rim);
				return _Color * _Color.a + rim * mainTex ;
			}
			ENDCG
		}
	}
}
