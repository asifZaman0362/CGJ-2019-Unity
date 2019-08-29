// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Effects/Outline" {
    Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("OutlineColor", Color) = (1,1,1,1)
		_RefColor ("Reference Color", Color) = (0, 0, 0, 0)
        _Size ("OutlineSize", Range(1, 10)) = 3
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
            fixed4 _MainTex_TexelSize;
            float4 _OutlineColor;
			float4 _RefColor;
            float _Size;

            bool checkNeighbours(float2 uv, int size) {
				float4 up = tex2D(_MainTex, uv + fixed2(0, _MainTex_TexelSize.y * size));
                float4 down = tex2D(_MainTex, uv - fixed2(0, _MainTex_TexelSize.y * size));
                float4 left = tex2D(_MainTex, uv - fixed2(_MainTex_TexelSize.x * size, 0));
                float4 right = tex2D(_MainTex, uv + fixed2(_MainTex_TexelSize.x * size, 0));
                //float4 comb = up*down*left*right;
                return up == _RefColor || down == _RefColor || left == _RefColor || right == _RefColor;
            }

			float4 frag (v2f i) : SV_Target
			{
                float4 col = tex2D(_MainTex, i.uv);
                if (col.r == _RefColor.r && col.g == _RefColor.g && col.b == _RefColor.b) return col;
                if (checkNeighbours(i.uv, _Size)) {
                    return _OutlineColor * abs(sin((i.uv.x + i.uv.y) * 10)) + float4(0, 1, 0, 1) * abs(sin(i.uv.x * 10)) + float4(0, 0, 1, 1) * abs(cos(i.uv.y * 10));
                }   
				return col;
			}
			ENDCG
		}
	}
}