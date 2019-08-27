Shader "CrossSection/OppositeStencil"
{
	Properties {
		_Color ("Color", Color) = (0, 0, 0, 1)
		_StencilMask("Stencil Mask", Range(0, 255)) = 255
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 100
		ZTest On
		Stencil{
			Ref [_StencilMask]
			Comp Greater
		}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct app_data
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			fixed4 _Color;
			
			v2f vert (app_data v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return _Color;
			}
			ENDCG
		}
	}
}