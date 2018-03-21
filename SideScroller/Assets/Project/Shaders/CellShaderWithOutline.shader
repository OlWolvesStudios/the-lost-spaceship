Shader "Custom/CellShaderWithOutline"
{
	Properties
	{
		_Color("Diffuse Material Color", Color) = (1,1,1,1)
		_UnlitColor("Unlit Color", Color) = (0.5,0.5,0.5,1)
		_DiffuseThreshold("Lighting Threshold", Range(-1.1,1)) = 0.1
		_SpecColor("Specular Material Color", Color) = (1,1,1,1)
		_Shininess("Shininess", Range(0.5,1)) = 1
		_MainTex("Main Texture", 2D) = "white" {}

		_OutlineColor("Color", Color) = (1,1,1,1)
		_OutlineWidth("Outline Width", Range(1,2)) = 1
		_Fade("Fade", Range(1,100)) = 10

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
			Pass
			{
				Tags{ "LightMode" = "ForwardBase" }

				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag

				uniform float4 _Color;
				uniform float4 _UnlitColor;
				uniform float _DiffuseThreshold;
				uniform float4 _SpecColor;
				uniform float _Shininess;
				uniform float _Fade;

				uniform float4 _LightColor0;
				uniform sampler2D _MainTex;
				uniform float4 _MainTex_ST;

				struct vertexInput
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 texcoord : TEXCOORD0;
				};

				struct vertexOutput
				{
					float4 pos : SV_POSITION;
					float3 normalDir : TEXCOORD1;
					float4 lightDir : TEXCOORD2;
					float3 viewDir : TEXCOORD3;
					float2 uv : TEXCOORD0;
				};

				vertexOutput vert(vertexInput input)
				{
					vertexOutput output;

					output.normalDir = normalize(mul(float4(input.normal, 0.0), unity_WorldToObject).xyz);

					float4 posWorld = mul(unity_ObjectToWorld, input.vertex);

					output.viewDir = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz);

					float3 fragmentToLightSource = (_WorldSpaceCameraPos.xyz - posWorld.xyz);

					output.lightDir = float4(normalize(lerp(_WorldSpaceLightPos0.xyz , fragmentToLightSource, _WorldSpaceLightPos0.w)), lerp(1.0 , 1.0 / length(fragmentToLightSource), _WorldSpaceLightPos0.w));

					output.pos = UnityObjectToClipPos(input.vertex);

					output.uv = input.texcoord;

					return output;
				}

				float4 frag(vertexOutput input) : COLOR
				{
					float nDotL = saturate(dot(input.normalDir, input.lightDir.xyz));

					float diffuseCutoff = saturate((max(_DiffuseThreshold, nDotL) - _DiffuseThreshold) * _Fade);

					float specularCutoff = saturate((max(_Shininess, nDotL) - _Shininess) * _Fade);

					float3 ambientLight = (1 - diffuseCutoff) * _UnlitColor.xyz;
					float3 diffuseReflection = (1 - specularCutoff) * _Color.xyz * diffuseCutoff;
					float3 specularReflection = _SpecColor.xyz * specularCutoff;

					float3 combinedLight = ((ambientLight + diffuseReflection + specularReflection) / 1) * _LightColor0.rgb; // If we want light color multiply all together by _LightColor0.rgb

					return float4(combinedLight, 1.0) * tex2D(_MainTex, input.uv);
				}
				ENDCG
			}
		}

		Fallback "Diffuse"
}
