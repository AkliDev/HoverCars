Shader "Custom/ColorMask" {
	Properties{
	_Color("Color", Color) = (1,1,1,1)
	_MainTex("Albedo (RGB)", 2D) = "white" {}
	[NoScaleOffset]_MaskTex("Mask (RGB)", 2D) = "White" {}
	_EmissionColor("Emissive Color", Color) = (1,1,1,1)
		[NoScaleOffset]	_Emission("Emisive",2D) = "black" {}
	[NoScaleOffset]	_Normal("Normal",2D) = "bump"{}
	_metalAmm("Metallic", Range(0,1)) = 0.0
	_GlossAmm("Smoothness", Range(0,1)) = 0.0
		[NoScaleOffset]_Glosiness("Glossiness", 2D) = "white"{}
	[NoScaleOffset]	_Metallic(	"Metallic", 2D) = "white"{}

	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		sampler2D _MainTex,_MaskTex,_Normal,_Metallic,_Glossiness,_Emission;
	fixed3 _color;

	struct Input {
		float2 uv_MainTex;
	};

	fixed4 _Color;
	fixed _metalAmm;
	fixed _GlossAmm;
	fixed4 _EmissionColor;

	void surf(Input IN, inout SurfaceOutputStandard o) {
		// Albedo comes from a texture tinted by color
		float4 c = tex2D(_MainTex, IN.uv_MainTex);
		float4 mask = tex2D(_MaskTex, IN.uv_MainTex);
		c.rgb = c.rgb * (1 - mask) + _Color * mask;

		o.Albedo = c.rgb;
		o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_MainTex));
		o.Metallic = tex2D(_Metallic, IN.uv_MainTex).rgb * _metalAmm;
		o.Smoothness = tex2D(_Metallic, IN.uv_MainTex).a * _GlossAmm;
		o.Emission = tex2D(_Emission, IN.uv_MainTex) * _EmissionColor;
		o.Alpha = c.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
	
}

// Made by Hicham Ouchan