// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#ifndef GAF_FILTERS_INCLUDED
#define GAF_FILTERS_INCLUDED

#include "GAFShaderBase.cginc"

////////////////////////////
// DEFINITIONS
////////////////////////////
uniform float _BlurX;
uniform float _BlurY;

uniform float _Strength;

uniform fixed4 _GlowColor;

uniform float2 _Scale;
uniform float2 _Pivot;

#if GAF_COLOR_MTX_FILTER_ON

uniform float4x4 _ColorMtx;
uniform fixed4	 _Offset;

#endif // GAF_COLOR_MTX_FILTER_ON

////////////////////////////
// STRUCTS
////////////////////////////

struct gaf_v2f_blur
{
	float4 position		: SV_POSITION;
	float2 texcoord		: TEXCOORD0;
	float2 blurstep		: TEXCOORD1;
};

////////////////////////////
// FUNCTIONS VERTEX
////////////////////////////

gaf_v2f_base gaf_filter_vert(appdata_base input)
{
	gaf_v2f_base output;

	input.vertex.xy -= _Pivot;
	input.vertex.xy *= _Scale.xy;
	input.vertex.xy += _Pivot;
	output.position	= UnityObjectToClipPos (input.vertex);

	output.texcoord		= TRANSFORM_TEX(input.texcoord, _MainTex);
	output.color		= _TintColor;
	output.colorOffset	= _TintColorOffset;

	return output;
}

gaf_v2f_blur gaf_blur_vert(appdata_base input)
{
	gaf_v2f_blur output;

	output.position	= UnityObjectToClipPos (input.vertex);
	output.texcoord	= TRANSFORM_TEX(input.texcoord, _MainTex);
	output.blurstep = float2(_BlurX * (_MainTex_TexelSize.x), _BlurY * (_MainTex_TexelSize.y));

	return output;
}

////////////////////////////
// FUNCTIONS FRAGMENT
////////////////////////////

fixed4 gaf_filter_frag(gaf_v2f_base input) : SV_Target
{
	fixed4 resultColor = gaf_frag(input);

#if GAF_COLOR_MTX_FILTER_ON
	return mul(_ColorMtx, resultColor) + _Offset;
#else
	return resultColor;
#endif // GAF_COLOR_MTX_FILTER_ON
}

fixed4 gaf_blur_frag(gaf_v2f_blur input) : SV_Target
{
	fixed4 resultColor = fixed4(0, 0, 0, 0);

	resultColor += tex2D(_MainTex, input.texcoord - input.blurstep * 4.0) * 0.05;
	resultColor += tex2D(_MainTex, input.texcoord - input.blurstep * 3.0) * 0.09;
	resultColor += tex2D(_MainTex, input.texcoord - input.blurstep * 2.0) * 0.12;
	resultColor += tex2D(_MainTex, input.texcoord - input.blurstep * 1.0) * 0.15;
	resultColor += tex2D(_MainTex, input.texcoord + input.blurstep * 0.0) * 0.18;
	resultColor += tex2D(_MainTex, input.texcoord + input.blurstep * 1.0) * 0.15;
	resultColor += tex2D(_MainTex, input.texcoord + input.blurstep * 2.0) * 0.12;
	resultColor += tex2D(_MainTex, input.texcoord + input.blurstep * 3.0) * 0.09;
	resultColor += tex2D(_MainTex, input.texcoord + input.blurstep * 4.0) * 0.05;
    
	return resultColor;
}

fixed4 gaf_glow_frag(gaf_v2f_blur input) : SV_Target
{
	fixed4 resultColor = fixed4(0, 0, 0, 0);

	resultColor += tex2D(_MainTex, input.texcoord - input.blurstep * 4.0).a * 0.05;
	resultColor += tex2D(_MainTex, input.texcoord - input.blurstep * 3.0).a * 0.09;
	resultColor += tex2D(_MainTex, input.texcoord - input.blurstep * 2.0).a * 0.12;
	resultColor += tex2D(_MainTex, input.texcoord - input.blurstep * 1.0).a * 0.15;
	resultColor += tex2D(_MainTex, input.texcoord + input.blurstep * 0.0).a * 0.18;
	resultColor += tex2D(_MainTex, input.texcoord + input.blurstep * 1.0).a * 0.15;
	resultColor += tex2D(_MainTex, input.texcoord + input.blurstep * 2.0).a * 0.12;
	resultColor += tex2D(_MainTex, input.texcoord + input.blurstep * 3.0).a * 0.09;
	resultColor += tex2D(_MainTex, input.texcoord + input.blurstep * 4.0).a * 0.05;

	resultColor.a *= _Strength;
    
	return resultColor * _GlowColor;
}

#endif
