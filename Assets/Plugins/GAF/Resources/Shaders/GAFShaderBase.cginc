// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#ifndef GAF_SHADER_BASE_INCLUDED
#define GAF_SHADER_BASE_INCLUDED

#include "UnityCG.cginc"

////////////////////////////
// DEFINITIONS
////////////////////////////

sampler2D 		_MainTex;
float4 			_MainTex_ST;
uniform float4	_MainTex_TexelSize;

float4 _TintColor;
float4 _TintColorOffset;

fixed4 _CustomColorMultiplier;
fixed4 _CustomColorOffset;

////////////////////////////
// STRUCTS
////////////////////////////

struct gaf_v2f_minimal
{
	float4 position		: SV_POSITION;
	float2 texcoord		: TEXCOORD0;
};

struct appdata
{
	float4 vertex		: POSITION;  // The vertex position in model space.
	float4 texcoord		: TEXCOORD0; // The first UV coordinate.
	float4 uv1	 		: TEXCOORD1; // The second UV coordinate.
	float4 uv2 			: TEXCOORD2; // The second UV coordinate.
	float4 color		: COLOR;     // Per-vertex color
};

struct gaf_v2f_base
{
	float4 position		: SV_POSITION;
	float2 texcoord		: TEXCOORD0;
	fixed4 color : COLOR;
	fixed4 colorOffset : TEXCOORD1;
};

////////////////////////////
// FUNCTIONS VERTEX
////////////////////////////

gaf_v2f_base gaf_base_vert_group(appdata input)
{
	gaf_v2f_base output;

#if GAF_VERTICES_TRANSFORM_ON
	output.position = UnityObjectToClipPos(input.vertex);
#else
	output.position = input.vertex;

#if !UNITY_UV_STARTS_AT_TOP
	output.position.y = -output.position.y;
#endif // UNITY_UV_STARTS_AT_TOP

#endif // GAF_VERTICES_TRANSFORM_ON

	output.texcoord = TRANSFORM_TEX(input.texcoord, _MainTex);
	output.color = input.color;
	output.colorOffset = fixed4(input.uv1.x, input.uv1.y, input.uv2.x, input.uv2.y);

	return output;
}

gaf_v2f_minimal gaf_minimal_vert(appdata_base input)
{
	gaf_v2f_minimal output;

	output.position = UnityObjectToClipPos(input.vertex);
	output.texcoord = TRANSFORM_TEX(input.texcoord, _MainTex);

	return output;
}

////////////////////////////
// FUNCTIONS FRAGMENT
////////////////////////////

fixed4 gaf_frag(gaf_v2f_base input)
{
	return (tex2D(_MainTex, input.texcoord) * input.color + input.colorOffset) * _CustomColorMultiplier + _CustomColorOffset;
}

fixed4 gaf_base_frag(gaf_v2f_base input) : SV_Target
{
	return gaf_frag(input);
}

fixed4 gaf_mask_frag(gaf_v2f_minimal input) : SV_Target
{
	fixed4 resultColor = tex2D(_MainTex, input.texcoord);

if (resultColor.a < 0.01)
{
	discard;
}

return resultColor;
}
#endif
