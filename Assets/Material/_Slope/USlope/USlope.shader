Shader "Custom/USlope"
{
    Properties
    {
        _LineColor ("LineColor", Color) = (1,1,1,1)
        _MainColor ("MainColor", Color) = (0,0,0,1)

		_LatticeSize("Lattice Size", float) = 10
		_LineWidth("Line Width", float) = 0.1
		_EdgeWidth("Edge Width", float) = 0.3 

		_MinY("MinY", float) = 0
		_MaxY("MaxY", float) = 0

		_NY ("NY", float) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
		#pragma surface surf Standard 
		#pragma target 3.0

		half4 _LineColor;
		half4 _MainColor;

		half _LatticeSize;
		half _LineWidth;
		half _EdgeWidth;

		half _MinY;
		half _MaxY;

		half _NY;

		half delta(half pos, half size)
		{
			return pos - round(pos / size) * size;
		}

		struct Input
		{
			half3 worldPos;
			half3 worldNormal;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			if (IN.worldNormal.y > _NY)
			{
				half mdy = IN.worldPos.y - _MinY;
				if (mdy < _EdgeWidth) { o.Albedo = _LineColor; return; }

				half yLim = _MaxY - _EdgeWidth; 
				if (IN.worldPos.y > yLim) { o.Albedo = _LineColor; return; }

				half dz = abs(delta(IN.worldPos.z, _LatticeSize));
				half dy = abs(delta(IN.worldPos.y, _LatticeSize));

				if (dz < _LineWidth || dy < _LineWidth)
				{
					o.Albedo = _LineColor;
				}

				else
				{
					o.Albedo = _MainColor;
				}
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
