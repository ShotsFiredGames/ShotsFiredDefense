// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/ScannerEffect"
{
	Properties{
			_Color("Color", Color) = (1, 1, 1, 1)
	}
	SubShader {

		Blend SrcAlpha OneMinusSrcAlpha
		Pass {
		
			Name "SCANNEREFFECT"

			Tags { "RenderType"="Opaque" }
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				float3 worldPos : TEXCOORD1;
			};


			v2f vert(appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				//o.color = i.texcoord1;
				return o;
			}
			
			 float4 _Color;
			 float3 _Centers[9];
			 float _Strengths[9];
			 float _Radius[9];
			 

			float4 frag(v2f i) : SV_Target {
				float preVal = 0;
				float val = 0;
				_Color.r = 1; 
				_Color.g = _Color.b = 0; 
				for (int j = 0; j < 9; j++)
				{
					float dist = distance(_Centers[j], i.worldPos);
					//val = 1 - step(dist, diff * 0.1) * 0.5;
					//val = step(diff * 1.5, dist) * step(dist, _Radius[j]) * val;
					float inner = _Radius[j] * .09 * _Strengths[j];
					float outer = _Radius[j] * .1 * _Strengths[j];
					if (_Radius[j] == 0)
						continue;
					val = sin(3.14159*(dist - inner) / (outer - inner));
					
					val = step(inner, dist) * step(dist, outer)* val;

					//val = step(0,j-1) * step(.001, val + preVal);// *lerp(1, 0, dist / 180);
					preVal += val;
				}
				return float4(preVal * _Color.r, preVal * _Color.g, preVal * _Color.b, 1);
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
