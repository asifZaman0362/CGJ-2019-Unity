Shader "CrossSection/Unlit" {

    Properties {

        _Color ("Color", Color) = (1, 1, 1, 1)
        _CrossSectionColor ("Cross Section Color", Color) = (1, 1, 1, 1)
        _PlanePosition ("Plane position", Vector) = (0, 0, 0, 1)
        _PlaneNormal ("Plane normal", Vector) = (0, 0, 0, 1)
        _StencilMask ("Stencil Mask", Range(0, 255)) = 255

    }

    SubShader {
        Tags {"RenderType" = "Opaque"}
        Stencil {
            Ref [_StencilMask]
            CompBack Always
            PassBack Replace
            CompFront Always
            PassFront Zero
        }
        Cull Back

        CGPROGRAM

        #pragma surface surf Lambert
        //#pragma target 3.0

        struct Input {
            float3 worldPos;
        };

        fixed4 _Color;
        fixed4 _CrossSectionColor;
        fixed3 _PlaneNormal;
        fixed3 _PlanePosition;

        bool isVisible(fixed3 worldPos) {
            float worldDotPlane = dot(worldPos - _PlanePosition, _PlaneNormal);
            return worldDotPlane > 0;
        }

        void surf (Input i, inout SurfaceOutput o) {
            if (isVisible(i.worldPos)) discard;
            fixed4 col = _Color;
            o.Albedo = col.rgb;
        }

        ENDCG

        Cull Front 

        CGPROGRAM

        #pragma surface surf NoLighting

        struct Input {
            fixed3 worldPos;
        };
        fixed4 _Color;
        fixed4 _CrossSectionColor;
        fixed3 _PlaneNormal;
        fixed3 _PlanePosition;

        bool isVisible(fixed3 worldPos) {
            float worldDotPlane = dot(worldPos - _PlanePosition, _PlaneNormal);
            return worldDotPlane > 0;
        }

        fixed4 LightingNoLighting(SurfaceOutput o, fixed3 lightDir, fixed attenuation) {
            fixed4 color;
            color.rgb = o.Albedo;
            color.a = o.Alpha;
            return color;
        }

        void surf(Input i, inout SurfaceOutput o) {
            if (isVisible(i.worldPos)) discard;
            o.Albedo = _CrossSectionColor;
        }

        ENDCG

    }

}