Shader "Custom/CharacterOutline"
{
	Properties
	{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Texture", 2D) = "white" {}
		_OutlineColor("Color", Color) = (1,1,1,1)
		_Outline("Outline Width 1", Range(.002, 0.03)) = .005
		_OutlineWidth("Outline Width 2", Range(1,2)) = 1
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
			Tags{ "Queue" = "Transparent" }

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

			// Reder second outline
		Pass
		{
			Cull Front

			CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		struct v2f2
		{
			float4 pos : SV_POSITION;
		};

		float _Outline;

		float4 vert(appdata_base v) : SV_POSITION
		{
			v2f2 o;
			o.pos = UnityObjectToClipPos(v.vertex);
			float3 normal = mul((float3x3) UNITY_MATRIX_MV, v.normal);
			normal.x *= UNITY_MATRIX_P[0][0];
			normal.y *= UNITY_MATRIX_P[1][1];
			o.pos.xy += normal.xy * _Outline;
			return o.pos;
		}

			half4 frag(v2f2 i) : COLOR
			{
				return _OutlineColor;
			}

			ENDCG
		}

		// Normal Render
		Pass
		{
			ZWrite On

			Material
			{
				Diffuse[_Color]
				Ambient[_Color]
			}

			Lighting On

			SetTexture[_MainTex]
			{
			ConstantColor[_Color]
			}

			SetTexture[_MainTex]
			{
				Combine previous * primary DOUBLE
			}
		}
	}
}