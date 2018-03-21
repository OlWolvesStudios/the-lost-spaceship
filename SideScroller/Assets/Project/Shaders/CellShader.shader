Shader "Custom/CellShader" 
{
	Properties
	{
		_Color("Diffuse Material Color", Color) = (1,1,1,1)
		_UnlitColor("Unlit Color", Color) = (0.5,0.5,0.5,1)
		_DiffuseThreshold("Lighting Threshold", Range(-1.1,1)) = 0.1
		_SpecColor("Specular Material Color", Color) = (1,1,1,1)
		_Shininess("Shininess", Range(0.5,1)) = 1
		_MainTex("Main Texture", 2D) = "white" {}

	}

		SubShader
		{
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

					//normalDirection
					output.normalDir = normalize(mul(float4(input.normal, 0.0), unity_WorldToObject).xyz);

					//World position
					float4 posWorld = mul(unity_ObjectToWorld, input.vertex);

					//view direction
					output.viewDir = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz); //vector from object to the camera

																						 //light direction
					float3 fragmentToLightSource = (_WorldSpaceCameraPos.xyz - posWorld.xyz);

					output.lightDir = float4
					(
						normalize(lerp(_WorldSpaceLightPos0.xyz , fragmentToLightSource, _WorldSpaceLightPos0.w)),
						lerp(1.0 , 1.0 / length(fragmentToLightSource), _WorldSpaceLightPos0.w)
					);

					//fragmentInput output;
					output.pos = UnityObjectToClipPos(input.vertex);

					//UV-Map
					output.uv = input.texcoord;

					return output;
				}

				float4 frag(vertexOutput input) : COLOR
				{
					float nDotL = saturate(dot(input.normalDir, input.lightDir.xyz));

					//Diffuse threshold calculation
					float diffuseCutoff = saturate((max(_DiffuseThreshold, nDotL) - _DiffuseThreshold) * 10);

					//Specular threshold calculation
					float specularCutoff = saturate((max(_Shininess, nDotL) - _Shininess) * 10);

					float3 ambientLight = (1 - diffuseCutoff) * _UnlitColor.xyz * _LightColor0.rgb; //adds general ambient illumination
					float3 diffuseReflection = (1 - specularCutoff) * _Color.xyz * diffuseCutoff * _LightColor0.rgb;
					float3 specularReflection = _SpecColor.xyz * specularCutoff *_LightColor0.rgb;

					float3 combinedLight = ambientLight + diffuseReflection + specularReflection; // If we want light color multiply all together by _LightColor0.rgb

					return float4(combinedLight, 1.0) * tex2D(_MainTex, input.uv); // DELETE LINE COMMENTS & ';' TO ENABLE TEXTURE
				}
				ENDCG
			}
		}

		Fallback "Diffuse"
}