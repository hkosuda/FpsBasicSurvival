Shader "Custom/SimpleChess"
{
    Properties
    {
        _Color1 ("Color1", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (0,0,0,1)

        _ChessSize ("ChessSize", float) = 5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
		#pragma surface surf Standard 
		#pragma target 3.0

		half4 _Color1;
		half4 _Color2;

		half _ChessSize;

		half chess(half x, half y)
		{
			half row = round(x / _ChessSize);
			half col = round(y / _ChessSize);

			if ((row + col) % 2 == 0)
			{
				return 1;
			}

			else
			{
				return -1;
			}
		}

		struct Input
		{
			half3 worldPos;
			half3 worldNormal;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			if (abs(IN.worldNormal.y) > 0.99)
			{
				if (chess(IN.worldPos.x, IN.worldPos.z) > 0)
				{
					o.Albedo = _Color2;
				}

				else
				{
					o.Albedo = _Color1;
				}
			}

			else if (abs(IN.worldNormal.z) > 0.99)
			{
				if (chess(IN.worldPos.x, IN.worldPos.y) > 0)
				{
					o.Albedo = _Color1;
				}

				else
				{
					o.Albedo = _Color2;
				}
			}

			else
			{
				if (chess(IN.worldPos.y, IN.worldPos.z) > 0)
				{
					o.Albedo = _Color1;
				}

				else
				{
					o.Albedo = _Color2;
				}
			}
		}

	ENDCG
    }
    FallBack "Diffuse"
}
