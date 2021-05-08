Shader "Toon/Toon Normal"
{
    Properties
    {
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
        _MainTex ("Main Texture", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "" {}
		_Steps ("Steps", Range(2, 10)) = 4
		_AmbientColor("Ambient Color", Color) = (0.5,0.5,0.5,1)
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Glossiness("Glossiness", Float) = 32
		_NormalIntensity("Normal Depth", Range(0,10)) = 1
    }
    SubShader
    {
        Tags {
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

            #include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
				float3 tangent : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
				float3 worldNormal : NORMAL;
				float3 ldir : TEXTCOORD1;
				float3 viewDir : TEXCOORD1;
                float2 uv : TEXCOORD0;
				SHADOW_COORDS(2)
            };

            uniform sampler2D _MainTex;
            uniform float4 _Color;
			uniform sampler2D _NormalMap;
			uniform float _NormalIntensity;
			uniform int _Steps;
			uniform float4 _AmbientColor;
			uniform float _Glossiness;
			uniform float4 _SpecularColor;
			uniform float4 _RimColor;
			uniform float _RimAmount;
			uniform float _RimThreshold;
			float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				
				
				//o.viewDir = WorldSpaceViewDir(v.vertex);
				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(UNITY_MATRIX_M, v.vertex).xyz);

				o.ldir = normalize(_WorldSpaceLightPos0.xyz);

				float3 n = normalize(mul(UNITY_MATRIX_M, v.normal)); //normalize
				float3 t = normalize(mul(UNITY_MATRIX_M, v.tangent)); //Tangent
				float3 b = normalize(cross(n, t)); //cross Vector Normal&&Tangent

				float3x3 tbn = float3x3(t, b, n); //TBN Matrix World to tangent space

				o.ldir = mul(tbn, o.ldir);
				o.viewDir = mul(tbn, o.viewDir);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float3 normal = UnpackNormal(tex2D(_NormalMap, i.uv)); //Normal from Normalmap
				normal.xy *= _NormalIntensity; //Normal Depth calc
				normal = normalize(normal); //normalise the world normals

				float4 sample = tex2D(_MainTex, i.uv); // sample the texture

				float NdotL = dot(i.ldir, normal); //Dot between light and normal
				float3 viewDir = normalize(i.viewDir);
				
				
				//Shadow
				float shadow = SHADOW_ATTENUATION(i);
				//Steps
				float lightIntensity = floor(NdotL * _Steps) / (float)_Steps;
				//float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);
				
				float4 light = lightIntensity * _LightColor0; //Light effects

				//specular reflection
				
				float3 halfVector = normalize(i.ldir + viewDir);
				float NdotH = dot(normal, halfVector);					
				//Toonify the specular
				float specularIntensity = pow(NdotH * shadow, _Glossiness * _Glossiness);
				specularIntensity = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensity * _SpecularColor;


                return _Color * sample * (_AmbientColor + light + specular);
            }
            ENDCG
        }
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
