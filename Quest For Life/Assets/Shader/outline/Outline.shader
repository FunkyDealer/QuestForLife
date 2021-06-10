Shader "Toon/Outline"
{
    Properties
    {
		_color ("Tint", Color) = (0,0,0,1)
        _MainTex ("Texture", 2D) = "white" {}

		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_outlineThickness ("Outline Thickness", Range(0, 1)) = 0.03


    }
    SubShader
    {
        Tags {
		"RenderType"="Opaque"
		"Queue"="Geometry"
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			//tint of the texture
			float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);
				//col *= _Color;
                return col;
            }
            ENDCG
        }

		//Second Pass for outlines
		Pass
		{
		Name "Outline"
		Cull front

		CGPROGRAM

		#include "UnityCG.cginc"

		#pragma vertex vert
		#pragma fragment frag

		 //the object data that's available to the vertex shader
            struct appdata
			{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            //the data that's used to generate fragments and can be read by the fragment shader
            struct v2f
			{
                float4 position : SV_POSITION;
            };

				

			uniform fixed4 _OutlineColor;
			uniform float _outlineThickness;

			v2f vert(appdata v)
			{
				v2f o;			
				//calculate the position of the expanded object
				float3 normal = normalize(v.normal);

				v.vertex *= (1 + _outlineThickness);

				//alternate way
				//float3 outlineOffset = normal * _outlineThickness;
				//float3 position = v.vertex + outlineOffset;

				//convert the vertex positions from object space to clip space so they can be rendered
				o.position = UnityObjectToClipPos(v.vertex);
				
				return o;
			}
		
			fixed4 frag(v2f i) : SV_TARGET 
			{
				return _OutlineColor;
			}

			ENDCG
		}

    }
}
