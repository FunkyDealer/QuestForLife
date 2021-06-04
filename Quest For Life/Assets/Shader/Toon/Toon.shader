Shader "toon/Toon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color", color) = (1,1,1,1)
		_Steps ("Steps", Range(1, 10)) = 4
		_ambientPower ("AmbientPower", Range(0,1)) = 0.1
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Shininess("Shininess", int) = 10
    }
    SubShader
    {
		Tags
		{
		"RenderType"="Opaque"
		}

        Pass
        {
		    Tags
			{ 
			"LightMode" = "ForwardBase" //tells that this pass is the base pass
			}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_fwdbase
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

            struct appdata
            {
                float4 pos : POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
				float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 world : TEXCOORD3;
                UNITY_FOG_COORDS(1)     
				SHADOW_COORDS(2)
            };

            sampler2D _Sampler;
            uniform sampler2D _MainTex;
			uniform float _Steps;
			uniform float4 _Color;
			uniform float _ambientPower;
			uniform float4 _SpecularColor;
			uniform int _Shininess;
			float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.normal = normalize(mul(float4(v.normal, 0), unity_WorldToObject).xyz); 

				o.world = mul(unity_ObjectToWorld, v.pos);

				TRANSFER_SHADOW(o);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }



            fixed4 frag (v2f i) : SV_Target
            {
				float3 lightDirection;
				float atten;
				float4 world = normalize(i.world);

				if (_WorldSpaceLightPos0.w == 0.0) //directional Lighting
				{
					atten = 1.0;
					lightDirection = normalize( _WorldSpaceLightPos0);
				}
				else //Point light
				{
					float3 framentToLightSource = _WorldSpaceLightPos0.xyz - i.world.xyz;
					float distance = length(framentToLightSource);
					atten = 1/distance;
					lightDirection = normalize(framentToLightSource);
				}					

                // sample the texture
			   float4 sample = tex2D(_MainTex, i.uv);

			    //Normalize Normals
				i.normal = normalize(i.normal);
				//FOR LIGHT SHADING
				float ndotl = dot(i.normal, lightDirection);

				//Shadow
				float shadow = SHADOW_ATTENUATION(i);

				///TOON SHADING
				//better toon shading without else ifs
				ndotl = (floor(ndotl * _Steps) / _Steps); 
				//ambient Color
				float lightIntensity = max(_ambientPower, ndotl * atten)  * shadow; 

				float4 light = lightIntensity * _LightColor0; //Light effects
				
				//Specular light
				float3 h = normalize(_WorldSpaceCameraPos.xyz - world);
				float NdotH = max(_ambientPower, dot(i.normal, h));	
				float4 specularIntensity = pow(NdotH * shadow, _Shininess * _Shininess);
				//Toonify the specular 
				specularIntensity = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensity * _SpecularColor;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, sample);

				fixed4 color = (sample * _Color) * (light + specular);
				float4 FinalColor = float4(color.rgb, _Color.a);
                return FinalColor;
            }
            ENDCG
        }
		Pass
        {
			 Tags
			{ 
				"LightMode" = "ForwardAdd" //tells that this pass is a add			 	
			}
			Blend One One //Determines how the GPU combines the output of the fragment shader with the render target.

            CGPROGRAM
			
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_fwdbase
            // make fog work
            #pragma multi_compile_fog
			// Adds shadows to a foward add pass
			#pragma multi_compile_fwdadd_fullshadows 

            #include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"			

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
				float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 world : TEXCOORD3;
                UNITY_FOG_COORDS(1)     
				SHADOW_COORDS(2)
            };

            sampler2D _Sampler;
            uniform sampler2D _MainTex;
			uniform float _Steps;
			uniform float4 _Color;
			uniform float _ambientPower;
			uniform float4 _SpecularColor;
			uniform int _Shininess;
			float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.normal = normalize(mul(float4(v.normal, 0), unity_WorldToObject).xyz); 

				o.world = mul(unity_ObjectToWorld, v.vertex);

				TRANSFER_SHADOW(o);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float3 lightDirection;
				float atten;
				float4 world = normalize(i.world);

				if (_WorldSpaceLightPos0.w == 0.0) //directional Lighting
				{
					atten = 1.0;
					lightDirection = normalize( _WorldSpaceLightPos0);
				}
				else 
				{			
					float3 framentToLightSource = _WorldSpaceLightPos0.xyz - i.world.xyz;
					float distance = length(framentToLightSource);
					atten = 1/distance;
					lightDirection = normalize(framentToLightSource);
				}					

                // sample the texture
			   float4 sample = tex2D(_MainTex, i.uv);

			    //Normalize Normals
				i.normal = normalize(i.normal);
				//FOR LIGHT SHADING
				float ndotl = dot(i.normal, lightDirection);

				//Shadow
				float shadow = SHADOW_ATTENUATION(i);

				///TOON SHADING
				//better toon shading without else ifs
				ndotl = (floor(ndotl * _Steps) / _Steps); 
				//ambient Color
				float lightIntensity = max(_ambientPower, ndotl * atten)  * shadow; 

				float4 light = lightIntensity * _LightColor0; //Light effects
				
				float3 h = normalize(_WorldSpaceCameraPos.xyz - world);
				float NdotH = max(_ambientPower, dot(i.normal, h));	
				float4 specularIntensity = pow(NdotH * shadow, _Shininess * _Shininess);
				//Toonify the specular 
				specularIntensity = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensity * _SpecularColor;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, sample);

				fixed4 color = (sample * _Color) * (light + specular);
				float4 FinalColor = float4(color.rgb, _Color.a);
                return FinalColor;
            }
            ENDCG
        }

		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
