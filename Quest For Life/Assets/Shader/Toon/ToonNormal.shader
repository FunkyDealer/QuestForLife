Shader "toon/ToonNormal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color", color) = (1,1,1,1)
		_Steps ("Steps", Range(1, 10)) = 4

		[Normal]
		_NormalMap("Normal Map", 2D) = "White" {}
		_NormalIntensity("Normal Depth", Range(0,2)) = 1		
		
		_ambientPower ("AmbientPower", Range(0,1)) = 0.1
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Shininess("Shininess", int) = 10
    }
    SubShader
    {
        Tags
		{ 
			"RenderType"="Opaque"
			"LightMode" = "ForwardBase"
			"PassFlags" = "OnlyDirectional"
		}

        Pass
        {
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
				float3 tangent : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
				float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float3 ldir : TEXTCOORD1;
				float3 world : TEXCOORD3;
				float3 viewDir : TEXCOORD4;
                UNITY_FOG_COORDS(1)     
				SHADOW_COORDS(2)
            };

            sampler2D _Sampler;
            uniform sampler2D _MainTex;
			uniform float _Steps;
			uniform float4 _Color;
			uniform sampler2D _NormalMap;
			uniform float _NormalIntensity;
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
				o.ldir = normalize(_WorldSpaceLightPos0.xyz);

				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(UNITY_MATRIX_M, v.pos).xyz);

				o.world = mul(UNITY_MATRIX_M, v.pos).xyz;

				float3 n = normalize(mul(UNITY_MATRIX_M, v.normal)); //normalize
				float3 t = normalize(mul(UNITY_MATRIX_M, v.tangent)); //Tangent
				float3 b = normalize(cross(n, t)); //cross Vector Normal&&Tangent

				float3x3 tbn = float3x3(t, b, n); //TBN Matrix World to tangent space

				o.ldir = mul(tbn, o.ldir);
				o.viewDir = mul(tbn, o.viewDir);

				TRANSFER_SHADOW(o);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float3 world = normalize(i.world);	

                // sample the texture
			   float4 sample = tex2D(_MainTex, i.uv);

			    //Normalize Normals
				i.normal = normalize(i.normal);

				//Normal Mapping
				float3 normal = UnpackNormal(tex2D(_NormalMap, i.uv)); //Normal from Normalmap
				normal.xy *= _NormalIntensity; //Normal Depth calc
				normal = normalize(normal);

				//FOR LIGHT SHADING
				float ndotl = dot(normal, i.ldir);

				//Shadow
				float shadow = SHADOW_ATTENUATION(i);

				///TOON SHADING
				//better toon shading without else ifs
				ndotl = (floor(ndotl * _Steps) / _Steps); 
				//ambient Color
				float lightIntensity = max(_ambientPower, ndotl * shadow); 

				float4 light = lightIntensity * _LightColor0; //Light effects
				
				float3 h = normalize(_WorldSpaceCameraPos.xyz - world);
				float NdotH = max(_ambientPower, dot(normal, h));	
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
