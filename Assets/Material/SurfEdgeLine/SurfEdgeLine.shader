Shader "Custom/SurfEdgeLine"
{
    Properties
    {
        _EdgeColor ("Edge Color", Color) = (1,0,0,1)
        _MainColor ("Main Color", Color) = (1,1,1,1)

        _EdgeWidth ("Edge Width", float) = 0.1 

        _HalfX ("Half X", float) = 0.5 
        _HalfZ ("Half Z", float) = 0.5 

        _X ("X", float) = 0
        _Z ("Z", float) = 0 

        _CosY ("Cos Y", float) = 1
        _SinY ("Sin Y", float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200
        Cull off

        CGPROGRAM
        #pragma surface surf Standard
        #pragma target 3.0

        half4 _EdgeColor;
        half4 _MainColor;

        half _EdgeWidth;

        half _HalfX;
        half _HalfZ;

        half _X;
        half _Z;

        half _CosY;
        half _SinY;

        struct Input
        {
            half3 worldPos;
            half3 worldNormal;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            if (IN.worldNormal.y > 0.99)
            {
                half dx = IN.worldPos.x - _X;
                half dz = IN.worldPos.z - _Z;

                half x = dz * _SinY + dx * _CosY;
                half z = dz * _CosY - dx * _SinY;

                half xlim = _HalfX - _EdgeWidth;
                if (abs(x) > xlim) { o.Albedo = _EdgeColor; return; }

                half zlim = _HalfZ - _EdgeWidth;
                if (abs(z) > zlim) { o.Albedo = _EdgeColor; return; }

                o.Albedo = _MainColor;
            }

            else
            {
                o.Albedo = _MainColor;
            }
        }
    ENDCG
    }
    FallBack "Diffuse"
}
