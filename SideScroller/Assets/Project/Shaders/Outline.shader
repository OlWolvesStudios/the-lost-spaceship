Shader "Custom/Outline"
{
	Properties
	{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor("Color", Color) = (1,1,1,1)
		_OutlineWidth ("Outline Width", Range(1,2)) = 1
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

		// Reder the outline
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
