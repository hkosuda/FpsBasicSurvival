Shader "Custom/Surf"
{
    Properties
    {
        _SurfColor ("Surf Color", Color) = (1,0,0,1)
        _BodyColor ("Body Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200
        Cull off

        CGPROGRAM
        #pragma surface surf Standard
        #pragma target 3.0

        half4 _SurfColor;
        half4 _BodyColor;

        struct Input
        {
            half3 worldNormal;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            if (IN.worldNormal.y > 0.99)
            {
                o.Albedo = _SurfColor;
            }

            else
            {
                o.Albedo = _BodyColor;
            }
        }
    ENDCG
    }
    FallBack "Diffuse"
}
