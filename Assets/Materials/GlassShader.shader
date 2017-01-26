Shader "Unlit/GlassShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_Coord("Coord", Range(-10, 10)) = 0 // Coordonnée à faire varier pour déplacer la bande lumineuse
		_Wide("Wide", Range(0, 2)) = 1 // Epaisseur de la bande lumineuse
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
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				float3 worldPos = mul(UNITY_MATRIX_M, IN.vertex).xyz;
				OUT.vpos = mul(UNITY_MATRIX_MV, IN.vertex).xyz;
				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
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

			// Fonction pour calculer la nouvelle luminosité de la couleur du sprite
			float toneFunction(float x) {
				float a = 1;
				float b = 0.2;
				float c = 3;
				float d = 0.1;
				float e = 0.3;
				x *= x;
				return (x*(a*x + b)) / (x*(c*x + d) + e);
			}

			// Fonction pour délimiter la bande lumineuse
			float f(float x) {
				return -0.3*x;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

				fixed4 baseColor = c.rgba;
				float lum = baseColor.x * 0.3 + baseColor.y * 0.6 + baseColor.z * 0.1;
				float lum2 = toneFunction(lum) / toneFunction(1);
				fixed3 fColor = baseColor.rgb*lum2 / lum;
				c.rgb *= c.a;

				// Si on se trouve dans la bande lumineuse dessinée, on change la luminosité
				if ((IN.texcoord.y) < f(IN.texcoord.x + _Coord) && (IN.texcoord.y) > f(IN.texcoord.x + _Coord + _Wide)) {
					return fixed4(fColor, 1);
				}
				return baseColor;
			}
			ENDCG
		}
	}
}

