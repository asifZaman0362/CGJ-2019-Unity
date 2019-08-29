// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CrossSection/Stencil" {
    Properties {
        _Color ("MainColor", Color) = (1, 1, 1, 1)
        _StencilMask ("StencilMask", Range(0, 255)) = 255
        _UseRandom ("UseRandomColors", float) = 1
    }
    SubShader {
        Tags {"RenderType" = "Transparent" "Queue" = "Transparent"}
        ZTest On
        Stencil {
            Ref [_StencilMask]
            Comp Equal
        }
        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct app_data {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float3 wpos : TEXCOORD0;
            };

            fixed4 _Color;
            float _UseRandom;

            v2f vert(app_data v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.wpos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET {
                if (_UseRandom == 0) return _Color;
                fixed4 col;
                col.r = abs(sin(i.wpos.z));
                col.g = abs(cos(i.wpos.z) + cos(i.wpos.y*0.2));
                col.b = abs(sin(i.wpos.y*0.2) + cos(i.wpos.y*0.2));
                col.a = 1;
                return col;
            }

            ENDCG
        }
    }
}