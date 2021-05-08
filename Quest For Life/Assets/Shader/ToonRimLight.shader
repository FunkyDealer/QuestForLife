﻿Shader "Toon/ToonRimLight"
{
    Properties
    {
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
        _MainTex ("Main Texture", 2D) = "white" {}
		_Steps ("Steps", Range(2, 10)) = 4
		_AmbientColor("Ambient Color", Color) = (0.5,0.5,0.5,1)
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Glossiness("Glossiness", Float) = 32
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.5
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;
                float2 uv : TEXCOORD0;
				SHADOW_COORDS(2)
            };

            uniform sampler2D _MainTex;
            uniform float4 _Color;
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
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float3 normal = normalize(i.worldNormal); //normalise the world normals
				float4 sample = tex2D(_MainTex, i.uv); // sample the texture

				float NdotL = dot(_WorldSpaceLightPos0, normal); //Dot between light and normal
				float3 viewDir = normalize(i.viewDir);
				
				
				//Shadow
				float shadow = SHADOW_ATTENUATION(i);
				//Steps
				float lightIntensity = floor(NdotL * _Steps) / (float)_Steps;
				//float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);
				
				float4 light = lightIntensity * _LightColor0; //Light effects

				//specular reflection
				
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);					
				//Toonify the specular
				float specularIntensity = pow(NdotH * shadow, _Glossiness * _Glossiness);
				specularIntensity = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensity * _SpecularColor;

				//Rim Light
				//calculate the rim Lightning
				float4 rimDot = 1 - dot(viewDir, normal);
				float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
			    rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				float4 rim = rimIntensity * _RimColor;

                return _Color * sample * (_AmbientColor + light + specular + rim);
            }
            ENDCG
        }
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}