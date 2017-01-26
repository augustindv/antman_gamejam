Shader "Sprites/GlassShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_Power("Power", Range(0, 1)) = 0.5
		_Coord("Coord", Range(-100, 100)) = 15
		_Wide("Wide", Range(0, 100)) = 3
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#pragma target 2.0
	#pragma multi_compile _ PIXELSNAP_ON
	#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
	#include "UnityCG.cginc"

		float toneFunction(float x) {
		float a = 1;
		float b = 0.2;
		float c = 3;
		float d = 0.1;
		float e = 0.3;
		x *= x;
		return (x*(a*x + b)) / (x*(c*x + d) + e);
	}

	struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord  : TEXCOORD0;
		float3 vpos : TEXCOORD1;
	};

	fixed4 _Color;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
		OUT.texcoord = IN.c;
		OUT.color = IN.color * _Color;

		return OUT;
	}

	sampler2D _MainTex;
	sampler2D _AlphaTex;
	float _Power;
	float _Coord;
	float _Wide;

	fixed4 SampleSpriteTexture(float2 uv)
	{
		fixed4 color = tex2D(_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
		// get the color from an external texture (usecase: Alpha support for ETC1 on android)
		color.a = tex2D(_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

		return color;
	}

	float f1(float x) {
		return 2 * x + 2;
	}

	float f2(float x) {
		return -2 * x + 2;
	}

	float f3(float x) {
		return -0.5*x + 0.5;
	}

	float f4(float x) {
		return 0.5*x - 2.5;
	}

	float f5(float x) {
		return 2 * x - 4;
	}

	float f6(float x) {
		return -2 * x - 4;
	}

	float f7(float x) {
		return -0.5*x - 2.5;
	}

	float f8(float x) {
		return 0.5*x + 0.5;
	}


	fixed4 frag(v2f IN) : SV_Target
	{
		fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

	fixed4 baseColor = c.rgba;
	float lum = baseColor.x * 0.3 + baseColor.y * 0.6 + baseColor.z * 0.1;
	float lum2 = toneFunction(lum) / toneFunction(1);
	fixed3 fColor = baseColor.rgb*lum2 / lum;
	c.rgb *= c.a;

	if ((IN.texcoord.y) < f1(IN.texcoord.x + _Coord) && (IN.texcoord.y) > f1(IN.texcoord.x + _Coord + _Wide)) {
		return fixed4(fColor, 1);
	}
	return baseColor;//fixed4(0.5,1,1, 1);
	}
		ENDCG
	}
	}
}

