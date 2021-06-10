Shader "my/Water"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_WaveSpeed("Wave Speed", float) = 1.0
		_WaveAmp("Wave Amp", float) = 0.2
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_MainTex("Main Texture", 2D) = "white" {}
		_DistortStrength("Distort Strength", float) = 1.0
		_ExtraHeight("Extra Height", float) = 0.0
		_MovText("Move Text",float) = 1.0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
			}

			// Grab the screen behind the object into _BackgroundTexture
			GrabPass
			{
				"_BackgroundTexture"
			}

			// Background distortion
			Pass
			{

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				// Properties
				uniform sampler2D _BackgroundTexture;
				uniform sampler2D _NoiseTex;
				uniform float _DistortStrength;
				uniform float _WaveSpeed;
				uniform float _WaveAmp;
				uniform float _MovText;

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float4 pos : SV_POSITION;
					float4 grabPos : TEXCOORD0;
				};

				v2f vert(appdata v)
				{
					v2f o;
					// convert input clip pos
					o.pos = UnityObjectToClipPos(v.vertex);
					// use ComputeGrabScreenPos function from UnityCG.cginc
					// to get the correct texture coordinate
					o.grabPos = ComputeGrabScreenPos(o.pos);

					// distort based on bump map
					float noiseSample = tex2Dlod(_NoiseTex, float4(v.uv, 0, 0));
					o.grabPos.y += sin(_Time * _WaveSpeed * noiseSample) * _WaveAmp * _DistortStrength;
					o.grabPos.x += cos(_Time * _WaveSpeed * noiseSample) * _WaveAmp * _DistortStrength;

					return o;
				}

				float4 frag(v2f i) : COLOR
				{
					return tex2Dproj(_BackgroundTexture, i.grabPos);
				}

				ENDCG
			}

			Pass
			{
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#include "UnityCG.cginc"

				#pragma vertex vert
				#pragma fragment frag

				// Properties
				uniform float4 _Color;
				uniform float  _WaveSpeed;
				uniform float  _WaveAmp;
				uniform float _ExtraHeight;
				uniform float _MovText;
				uniform sampler2D _CameraDepthTexture;
				uniform sampler2D _NoiseTex;
				uniform sampler2D _MainTex;

					struct appdata
					{
						float4 vertex : POSITION;
						float2 uv : TEXCOORD1;
					};

					struct v2f
					{
						float4 pos : SV_POSITION;
						float2 uv : TEXCOORD0;
					};

					v2f vert(appdata v)
					{
						v2f o;
						// convert to world space
						o.pos = UnityObjectToClipPos(v.vertex);
						// apply wave animation
						float noiseSample = tex2Dlod(_NoiseTex, float4(v.uv, 0, 0));

						o.pos.y += sin(_Time * _WaveSpeed * noiseSample) * _WaveAmp + _ExtraHeight;
						o.pos.x += cos(_Time * _WaveSpeed * noiseSample) * _WaveAmp;

						// texture coordinates 
						o.uv = v.uv;

						return o;
					}

					float4 frag(v2f i) : COLOR
					{
						//Move Text
						float2 mov;
						if (_MovText >= 1.0f)
						{
							mov = i.uv + float2(1, 0) * _Time.x * _MovText;
						}
						else
						{
							mov = i.uv;
						}

						// sample main texture
						float4 albedo = tex2D(_MainTex, mov);

						float4 col = _Color * albedo;
						return col;
					}

					ENDCG
				}
		}
}
