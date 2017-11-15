Shader "Custom/PaintOn" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_NormalMap("Normal Map", 2D) = "" {}
		_DrawOnTex("DrawTexture", 2D) = ""{}
		_PaintNormal("Normal map of paint", 2D) = ""{}

   		_Parallax ("Height", Range (0.005, 0.08)) = 0.02
   	 	_ParallaxMap ("Heightmap (A)", 2D) = "black" {}

     	_Cube ("Cubemap", CUBE) = "" {}

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
		sampler2D _PaintNormal;
		float _Parallax;
		sampler2D _ParallaxMap;
		half _Glossiness;
		half _Metallic;

		samplerCUBE _Cube;
		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
			float2 uv2_DrawOnTex;
			float2 uv2_PaintNormal;
    		float3 viewDir;
			float3 worldRefl;
			INTERNAL_DATA

		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {

			half h = tex2D (_ParallaxMap, IN.uv2_DrawOnTex).w;
		    float2 offset = ParallaxOffset (h, _Parallax, IN.viewDir);
		    IN.uv2_DrawOnTex += offset ;
		    IN.uv2_PaintNormal  += offset ;

			fixed4 c = 0;

			fixed4 mainColor = tex2D(_MainTex, IN.uv_MainTex);

			fixed4 drawColor = tex2D(_DrawOnTex, IN.uv2_DrawOnTex);

			c = lerp(mainColor, drawColor, drawColor.a);
			c.a = mainColor.a + drawColor.a;
			o.Albedo = c.rgb;
			o.Normal = lerp(UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap)),UnpackNormal(tex2D(_PaintNormal, IN.uv2_PaintNormal)), drawColor.a) ;

			// Metallic and smoothness come from slider variables
			if (drawColor.a > 0.5){
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
         		o.Emission = texCUBE (_Cube, WorldReflectionVector (IN, o.Normal)).rgb;
			}
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
