Shader "Lil Crit/UVlessTextureMapping"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		[NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		#include "UnityCG.cginc"

		#define TRIPLANAR_CORRECT_PROJECTED_U

		sampler2D _MainTex;
		sampler2D _BumpMap;
		float4 _MainTex_ST;
		fixed4 _Color;
		half _Glossiness;
		half _Metallic;

		struct Input
		{
			float3 worldPos;
			float3 worldNormal; INTERNAL_DATA
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			// calculate triplanar blend
			half3 triblend = pow(abs(IN.worldNormal), 4);
			triblend /= max(dot(triblend, half3(1,1,1)), 0.0001);

			// calculate triplanar uvs
			// applying texture scale and offset values ala TRANSFORM_TEX macro
			float2 uvX = IN.worldPos.zy * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 uvY = IN.worldPos.xz * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 uvZ = IN.worldPos.xy * _MainTex_ST.xy + _MainTex_ST.zw;

			// offset UVs to prevent obvious mirroring
		#if defined(TRIPLANAR_UV_OFFSET)
			uvY += 0.33;
			uvZ += 0.67;
		#endif

			// minor optimization of sign(), prevents return value of 0
			half3 axisSign = IN.worldNormal < 0 ? -1 : 1;

			// flip UVs horizontally to correct for back side projection
		#if defined(TRIPLANAR_CORRECT_PROJECTED_U)
			uvX.x *= axisSign.x;
			uvY.x *= axisSign.y;
			uvZ.x *= -axisSign.z;
		#endif

			// albedo textures
			fixed4 colX = tex2D(_MainTex, uvX);
			fixed4 colY = tex2D(_MainTex, uvY);
			fixed4 colZ = tex2D(_MainTex, uvZ);
			fixed4 col = colX * triblend.x + colY * triblend.y + colZ * triblend.z;

			// tangent space normal maps
			fixed3 tnormalX = UnpackNormal(tex2D(_BumpMap, uvX));
			fixed3 tnormalY = UnpackNormal(tex2D(_BumpMap, uvY));
			fixed3 tnormalZ = UnpackNormal(tex2D(_BumpMap, uvZ));

			// flip normal maps' x axis to account for flipped UVs
		#if defined(TRIPLANAR_CORRECT_PROJECTED_U)
			tnormalX.x *= axisSign.x;
			tnormalY.x *= axisSign.y;
			tnormalZ.x *= -axisSign.z;
		#endif

			// swizzle world normals to match tangent space and apply Whiteout normal blend
			tnormalX = fixed3(tnormalX.xy + IN.worldNormal.zy, tnormalX.z * IN.worldNormal.x);
			tnormalY = fixed3(tnormalY.xy + IN.worldNormal.xz, tnormalY.z * IN.worldNormal.y);
			tnormalZ = fixed3(tnormalZ.xy + IN.worldNormal.xy, tnormalZ.z * IN.worldNormal.z);

			// swizzle tangent normals to match world normal and blend together
			fixed3 worldNormal = normalize(
				tnormalX.zyx * triblend.x +
				tnormalY.xzy * triblend.y +
				tnormalZ.xyz * triblend.z
				);

			o.Albedo = col.rgb * _Color;
			o.Alpha = col.a;
			//o.Normal = worldNormal;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}