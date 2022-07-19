Shader "Custom/RimOrb"
{
    Properties
    {
        _RimColor ("RimColor", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Transparent" }
        LOD 200


        CGPROGRAM
        #pragma surface surf Standard  alpha:fade
        #pragma target 3.0

        struct Input
        {
            float3 normalDir;
            float3 viewDir;
        };

        half4 _RimColor;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            half wave_coeff = 1.1 + 0.5 * sin(_Time * 50);
            half dir_coeff = 1 - 2 * pow(dot(IN.viewDir, o.Normal), wave_coeff);

            if (dir_coeff < 0)
            {
                dir_coeff = 0;
            }

            half3 color = _RimColor * dir_coeff;

            o.Albedo = color;
            o.Emission = _RimColor;
            o.Alpha = dir_coeff;
        }

        ENDCG
    }
    FallBack "Diffuse"
}