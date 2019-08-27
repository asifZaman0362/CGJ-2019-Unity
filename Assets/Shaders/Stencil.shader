// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CrossSection/Stencil" {
    Properties {
        _Color ("MainColor", Color) = (1, 1, 1, 1)
        _StencilMask ("StencilMask", Range(0, 255)) = 255
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
            };

            fixed4 _Color;

            v2f vert(app_data v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET {
                return _Color;
            }

            ENDCG
        }
    }
}