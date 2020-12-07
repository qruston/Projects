Shader "Lil Crit/Water"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}
		_HeightMap1("Flow (Height Map XY)", 2D) = "bump" {}
		_HeightMap2("Ripple (Height Map ZW)", 2D) = "bump" {}
		_Direction("Flow", vector) = (0, 0, 0, 0)
		_Amplitude("Amplitude", float) = 1.0
	}
	SubShader
	{
		//Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }		// Switch commenting TAG lines to enable transparency (1/2)
		Tags { "RenderType" = "Opaque" }
		LOD 150

		CGPROGRAM
		
		//#pragma surface surf Standard addshadow fullforwardshadows alpha	// Switch commenting surface lines to enable transparency (2/2)
		#pragma surface surf Standard addshadow fullforwardshadows
		#pragma vertex vert

		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _HeightMap1;
		sampler2D _HeightMap2;
		float4 _MainTex_ST;
		float4 _HeightMap1_ST;
		float4 _HeightMap2_ST;
		half4 _Direction;
		half _Amplitude;

		struct Input
		{
			float2 uv_MainTex;
			float3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 col = tex2D(_MainTex, (IN.worldPos.xz * _MainTex_ST.xy + _MainTex_ST.zw) + _Direction * _Time.y) * _Color;
			
			o.Albedo = col.rgb;
			o.Alpha = col.a;
		}

		struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
		};

		void vert(inout appdata vert)
		{
			fixed4 pos = tex2Dlod(_HeightMap1, float4(vert.vertex.xz * _HeightMap1_ST.xy + _HeightMap1_ST.zw, 0, 0) + half4(_Direction.xy, 0, 0) * _Time.y);
			pos += tex2Dlod(_HeightMap2, float4(vert.vertex.xz * _HeightMap2_ST.xy + _HeightMap2_ST.zw, 0, 0) + half4(_Direction.zw, 0, 0) * _Time.y);
			vert.vertex.y += (((pos.r - 0.5) * _Amplitude) - _Amplitude) / 2.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
