Shader "Custom/PlaneSlope"
{
    Properties
    {
        _LineColor ("LineColor", Color) = (1,1,1,1)
        _MainColor ("MainColor", Color) = (0,0,0,1)

		_LatticeSize ("Lattice Size", float) = 10
		_LineWidth ("Line Width", float) = 0.1
		_EdgeWidth ("Edge Width", float) = 0.3

		_SizeX ("Size X", Float) = 1 
		_SizeZ( "Size Z", Float) = 1 

		_NzMin ("Normal Z Min", Float) = 0
		_NzMax ("Normal Z Max", Float) = 1

		_CosX ("Cos X", Float) = 1 
		_SinX ("Sin X", Float) = 0

		_CosZ ("Cos Z", Float) = 1 
		_SinZ ("Sin Z", Float) = 0 

		_X ("X", Float) = 0 
		_Y ("Y", Float) = 0
		_Z ("Z", Float) = 0 
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

		half _SizeX;
		half _SizeZ;

		half _CosX;
		half _SinX;
		
		half _CosZ;
		half _SinZ;

		half _X;
		half _Y;
		half _Z;

		half3 localPos(half x, half y, half z)
		{
			half xx = x * _CosZ - y * _SinZ * _CosX + z * _SinZ * _SinX;
			half yy = x * _SinZ + y * _CosZ * _CosX - z * _CosZ * _SinX; 
			half zz = y * _SinX + z * _CosX;

			return half3(xx, yy, zz);
		}

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
			half n = IN.worldNormal.xyz;
			half ny = localPos(IN.worldNormal.x, IN.worldNormal.y, IN.worldNormal.z).y;

			if (ny > 0.98)
			{
				half dx = IN.worldPos.x - _X;
				half dy = IN.worldPos.y - _Y;
				half dz = IN.worldPos.z - _Z;

				half3 local = localPos(dx, dy, dz);
				
				// edge 
				
				if (abs(local.x) < _EdgeWidth) { o.Albedo = _LineColor; return; }
				if (abs(local.z) < _EdgeWidth) { o.Albedo = _LineColor; return; }

				half edgeLimX = _SizeX - _EdgeWidth;
				if (abs(local.x) > edgeLimX) { o.Albedo = _LineColor; return; }

				half edgeLimZ = _SizeZ - _EdgeWidth;
				if (abs(local.z) > edgeLimZ) { o.Albedo = _LineColor; return; }

				// lattice 

				half ndx = abs(delta(local.x, _LatticeSize));
				half ndz = abs(delta(local.z, _LatticeSize));

				if (ndx < _LineWidth || ndz < _LineWidth)
				{
					o.Albedo = _LineColor;
					return;
				}

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
