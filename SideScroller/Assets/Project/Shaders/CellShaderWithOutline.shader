Shader "Custom/CellShaderWithOutline"
{
	Properties
	{
		_Color("Unlit Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
		_Ramp("Shader Gradient", 2D) = "white" {}

		_OutlineColor("Color", Color) = (1,1,1,1)
		_OutlineWidth("Outline Width", Range(1,2)) = 1
	}

		CGINCLUDE
		#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 color : COLOR;
		};

		struct v2f
		{
			float4 pos : POSITION;
		};

		float  _OutlineWidth;
		float4 _OutlineColor;

		v2f vert(appdata v)
		{
			v2f o;
			v.vertex.xyz *= _OutlineWidth;

			o.pos = UnityObjectToClipPos(v.vertex);
			return o;
		}

		ENDCG

		SubShader
		{			
			// Reder first outline
			Pass
			{
				ZWrite off

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				half4 frag(v2f i) : COLOR
				{
					return _OutlineColor;
				}
				ENDCG
			}

			// Render Cell Shading
			Tags{ "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf ToonRamp

			sampler2D _Ramp;

			#pragma lighting ToonRamp exclude_path:prepass
			inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
			{
				#ifndef USING_DIRECTIONAL_LIGHT
				lightDir = normalize(lightDir);
				#endif

				half d = dot(s.Normal, lightDir)*0.5 + 0.5;
				half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

				half4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
				c.a = 0;
				return c;
			}

			struct Input
			{
				float2 uv_MainTex : TEXCOORD0;
				float2 uv_BumpMap;
			};

			sampler2D _MainTex;
			sampler2D _BumpMap;
			float4 _Color;

			void surf(Input IN, inout SurfaceOutput o)
			{
				half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			}
			ENDCG
		}

		Fallback "Diffuse"
}
