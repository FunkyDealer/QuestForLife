Shader "TriangleDes/TimedDes"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)			//outputted Color
		_Speed("Speed", float) = 10					//Speed of fragments
		_maxDistance("max Distance", float) = 2		//max distance the fragments may travel
		_rotationSpeed ("RotationSpeed", float) = 5 //Their rotation speed
		_animTime("Animation Time", float) = 0	    //Extra time for animation
	}

	SubShader
	{
		Tags
		{
		"RenderType" = "Opaque"

		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag

			#include "UnityCG.cginc"
			
			uniform fixed _Speed;			//outputted Color
			uniform fixed _maxDistance;		//Speed of fragments
			uniform fixed4 _Color;			//max distance the fragments may travel
			uniform fixed _rotationSpeed;	//Their rotation speed
			uniform fixed _animTime;		//Extra time for animation

			uniform fixed startTime;		//Time at which the shader started running
					
			struct appdata 
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
            {
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

			struct g2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			// Return random value
			float rand(float2 seed)
			{
				return frac(sin(dot(seed.xy, float2(12.9898, 78.233))) * 43758.5453);
			}
	
			//create a rotation matrix
			float3x3 createRotation(float3 axis, float angle)
            {
				float3 n = axis; // the axis to rotate about
 
				// Specify the rotation transformation matrix:
				float3x3 m = float3x3(
				    n.x*n.x * (1.0f - cos(angle)) + cos(angle),       // column 1 of row 1
				    n.x*n.y * (1.0f - cos(angle)) + n.z * sin(angle), // column 2 of row 1
				    n.x*n.z * (1.0f - cos(angle)) - n.y * sin(angle), // column 3 of row 1
 
				    n.y*n.x * (1.0f - cos(angle)) - n.z * sin(angle), // column 1 of row 2
				    n.y*n.y * (1.0f - cos(angle)) + cos(angle),       // ...
				    n.y*n.z * (1.0f - cos(angle)) + n.x * sin(angle), // ...
 
				    n.z*n.x * (1.0f - cos(angle)) + n.y * sin(angle), // column 1 of row 3
				    n.z*n.y * (1.0f - cos(angle)) - n.x * sin(angle), // ...
				    n.z*n.z * (1.0f - cos(angle)) + cos(angle)        // ...
				);

				return m; 
            }

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = v.vertex;
				o.normal = normalize(v.normal);
				o.uv = v.uv;

				return o;
			}

			// Geometry Shader
			[maxvertexcount(3)]
			void geom(triangle v2f input[3], inout TriangleStream<g2f> stream)
			{
					float TimeSinceStart = _Time.y - startTime;
					TimeSinceStart = TimeSinceStart + _animTime; //give more time to the animation to skip no anim time

					//world coordinates
					float4 world0 = mul(unity_ObjectToWorld,input[0].pos);
					float4 world1 = mul(unity_ObjectToWorld,input[1].pos);
					float4 world2 = mul(unity_ObjectToWorld,input[2].pos);

					// The Center of the triangle
					float4 center = (world0 + world1 + world2) / 3;

					// Calculate the normal vector
					float3 vec1 = input[1].pos - input[0].pos;
					float3 vec2 = input[2].pos - input[0].pos;
					float3 normal = normalize(cross(vec1, vec2));

					//calculate the centroid vector for rotation
					float3 middle = (world0 + world1) / 2;
					float3 cent = middle - world2;
					cent = normalize(cent);

					//rotation matrix for rotating the triangles
					float3x3 rotation = createRotation(normal, _SinTime.y * _rotationSpeed);

					//Time function
					float destruction = clamp(TimeSinceStart * _Speed, 0.0, _maxDistance); 

					fixed random = rand(center.xy);
					fixed3 random3 = random.xxx;

					[unroll]
					for (int i = 0; i < 3; i++)
					{
						appdata v = input[i];
						g2f o;			

						// Move the vertex along the normal vector
						v.vertex.xyz += normal * destruction * random3;

						//Rotate Triangle
						float4 world = mul(unity_ObjectToWorld, v.vertex);

						world = float4(mul(rotation, world.xyz - center) + center,1);

						v.vertex = mul(unity_WorldToObject, world);

						o.vertex = UnityObjectToClipPos(v.vertex);
						o.uv = v.uv;

						o.color = _Color;
						stream.Append(o);
					}

					stream.RestartStrip();
				}

				fixed4 frag(g2f i) : SV_Target
				{
					fixed4 col = i.color;
					return col;
				}




				ENDCG
			}
	}
}
