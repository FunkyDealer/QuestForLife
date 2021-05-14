Shader "Unlit/Glass"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_EdgeColor("Edge Color", Color) = (1, 1, 1, 1) //Color of Edge
		_EdgeThickness("Silouette Dropoff Rate", float) = 1.0 //
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
			Cull Off
			//Controls whether pixels from this object are written to the depth buffer (default is On).
			//If you're drawng solid objects, leave this on. If you're drawing semitransparent effects, switch to ZWrite Off.
			ZWrite Off 
			Blend SrcAlpha OneMinusSrcAlpha // standard alpha blending


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
				float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float3 viewDir : TEXCOORD1;
            };

			uniform sampler2D _MainTex;
			uniform float4	_Color;
			uniform float4	_EdgeColor;
			uniform float   _EdgeThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
				float4 normal = float4(v.normal, 0.0);
				o.normal = normalize(mul(normal, unity_WorldToObject).xyz);

				o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
               
				o.uv = v.uv;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 texColor = tex2D(_MainTex, i.uv);

				// apply silouette equation
				// based on how close normal is to being orthogonal to view vector
				// dot product is smaller the smaller the angle bw the vectors is
				// close to edge = closer to 0
				// far from edge = closer to 1
				float edgeFactor = abs(dot(i.viewDir, i.normal));

				// apply edgeFactor to Albedo color & EdgeColor
				float oneMinusEdge = 1.0 - edgeFactor;
				float3 rgb = (_Color.rgb * edgeFactor) + (_EdgeColor * oneMinusEdge);
				rgb = min(float3(1, 1, 1), rgb); // clamp to real color vals
				rgb = rgb * texColor.rgb;

				// apply edgeFactor to Albedo transparency & EdgeColor transparency
				// close to edge = more opaque EdgeColor & more transparent Albedo 
				float opacity = min(1.0, _Color.a / edgeFactor);

				// opacity^thickness means the edge color will be near 0 away from the edges
				// and escalate quickly in opacity towards the edges
				opacity = pow(opacity, _EdgeThickness);
				opacity = opacity * texColor.a;

				float4 Color = float4(rgb, opacity);

                return Color;
            }
            ENDCG
        }
    }
}
