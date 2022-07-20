Shader "Custom/DashLine"
{
    Properties
    {
        _LineDirection("LineDirection", Float) = 1.0
        _MoveDirection("MoveDirection", Float) = 1.0
        _BasePosition("BasePosition", Float) = 1.0
        _Length("Length", Float) = 1.0
        _Ratio1("Ratio1", Float) = 0.0
        _Ratio2("Ratio2", Float) = 0.0
    }

        SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade
        #pragma target 3.0

        half _LineDirection;
        half _MoveDirection;
        half _BasePosition;
        half _Length;
        half _Ratio1;
        half _Ratio2;

        struct Input
        {
            float3 worldPos;
        };

        void surf(Input IN, inout SurfaceOutputStandard o) 
        {
            half position = 0.0;
            half dir_coeff = 1.0;

            half alpha = _Ratio2 - _Ratio1;
            half beta = _Ratio1;

            // direction : z
            if (_LineDirection > 0) {
                position = IN.worldPos.z;

                // positive direction
                if (_MoveDirection > 0) {
                    dir_coeff = -1.0;
                }

                // negative direction
                else {
                    dir_coeff = 1.0;
                }
            }

            // direction : x
            else {
                position = IN.worldPos.x;

                // positive direction
                if (_MoveDirection > 0) {
                    dir_coeff = -1.0;
                }

                // negative direction
                else {
                    dir_coeff = 1.0;
                }
            }

            half val = abs(sin(_Time * 150 * dir_coeff + position));

            if (val > 0.5 && _Length > 0) {
                half len_ratio = abs(position - _BasePosition) / _Length;
                half ratio = alpha * len_ratio + beta;

                half r = 2 * ratio - 1;
                half g = 1 - 2 * abs(ratio - 0.5);
                half b = 1 - 2 * ratio;
                
                //o.Albedo = fixed4(r, g, b, 1);
                o.Albedo = fixed4(0, 1, 0, 1);
                //o.Emission = fixed4(r, g, b, 1) * 1;
                o.Emission = fixed4(0, 1, 0, 1) * 1;
                o.Alpha = 1;
            }

            else {
                o.Albedo = fixed4(0, 1, 1, 0);
                o.Alpha = 0;
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
