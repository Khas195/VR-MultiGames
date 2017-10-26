Shader "Custom/PaintOn" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "" {}
		_DrawOnTex("DrawTexture", 2D) = ""{}

		_DoDissolve("Trigger Dissolve", int) = 1
		_DissolveTexture("Dissolve Texture", 2D) = "white"{}
		_DissolveY("Current Y  of the dissolve Effect", float) = 0
		_DissolveSize("Size of the effect", float) = 2
		_StartingY("Starting Point of the effect", float) = -10
	}
	SubShader {
		Tags { 
		"RenderType"="Opaque"
		"Glowable" = "True"
		  }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalMap;
		sampler2D _DrawOnTex;

		int _DoDissolve;
		sampler2D _DissolveTexture;
		float _DissolveY;
		float _DissolveSize;
		float _StartingY;
		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
			float2 uv2_DrawOnTex;
			float3 worldPos;
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {

			fixed4 c = 0;

			fixed4 mainColor = tex2D(_MainTex, IN.uv_MainTex);
			if (_DoDissolve == 1)
			{
				float transitionPoint = _DissolveY - IN.worldPos.y;
				// if clip receive a number >= 0 it will render
				clip(_StartingY + (transitionPoint + (tex2D(_DissolveTexture, IN.uv_MainTex) * _DissolveSize)));
				c = mainColor;
			}
			else {
				fixed4 drawColor = tex2D(_DrawOnTex, IN.uv2_DrawOnTex);

				c = lerp(mainColor, drawColor, drawColor.a);
				c.a = mainColor.a + drawColor.a;
			}
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
