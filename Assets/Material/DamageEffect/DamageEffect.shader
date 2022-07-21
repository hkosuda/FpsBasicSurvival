Shader "Unlit/DamageEffect"
{
    Properties
    {
        _SizeX ("Size X", Float) = 960
        _SizeY ("Size Y", Float) = 600

        _Width ("Width", Float) = 30
        _Alpha ("Alpha", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha 
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            half4 _Color;

            half _SizeX;
            half _SizeY;

            half _Width;
            half _Alpha;

            half getAlpha(half delta)
            {
                return 1 - pow(delta / _Width, 0.3);
            }

            half getAlphaXY(half dx, half dy)
            {
                if (dx < dy)
                {
                    return getAlpha(dx);
                }

                else
                {
                    return getAlpha(dy);
                }
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half x = i.vertex.x;
                half y = i.vertex.y;

                half xlim = _SizeX - _Width;
                half ylim = _SizeY - _Width;

                half alpha = 0;

                if (x < _Width)
                {
                    if (y < _Width)
                    {
                        alpha = getAlphaXY(x, y);
                    }

                    else if (y > ylim)
                    {
                        half dy = _SizeY - y; 
                        alpha = getAlphaXY(x, dy);
                    }

                    else
                    {
                        alpha = getAlpha(x);
                    }
                }

                else if (x > xlim)
                {
                    half dx = _SizeX - x;

                    if (y < _Width)
                    {
                        alpha = getAlphaXY(dx, y);
                    }

                    else if (y > ylim)
                    {
                        half dy = _SizeY - y; 
                        alpha = getAlphaXY(dx, dy);
                    }

                    else
                    {
                        alpha = getAlpha(dx);
                    }
                }

                else
                {
                    if (y < _Width)
                    {
                        alpha = getAlpha(y);
                    }

                    else if (y > ylim)
                    {
                        half dy = _SizeY - y; 
                        alpha = getAlpha(dy);
                    }

                    else
                    {
                        alpha = 0;
                    }
                }

                return half4(1, 0, 0, alpha * _Alpha);
            }
            ENDCG
        }
    }
}
