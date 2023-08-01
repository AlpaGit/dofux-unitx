Shader "GAF/GAFFilteredObject" 
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_StencilID ("Stencil ID", Float) = 0
		_CustomColorMultiplier ("Color multiplier", Color) = (1,1,1,1)
		_CustomColorOffset("Color offset", Vector) = (0,0,0,0)
		_TintColor ("Tint color multiplier", Color) = (1, 1, 1, 1)
		_TintColorOffset ("Tint color offset", Vector) = (0, 0, 0, 0)
		_Scale ("Post process scale", Vector) = (1, 1, 1, 1)
		_Pivot ("Mesh pivot", Vector) = (0, 0, 0, 0)
	}

	SubShader
	{
		Tags 
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
		}
		
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		Zwrite Off
		Lighting Off
		
		Stencil
		{
			Ref [_StencilID]
			Comp Equal
			Pass Keep
			Fail Keep
		}
		
		Pass 
		{
			CGPROGRAM

			#include "GAFFilters.cginc"

			#pragma multi_compile GAF_COLOR_MTX_FILTER_ON GAF_COLOR_MTX_FILTER_OFF			

			#pragma vertex gaf_filter_vert
			#pragma fragment gaf_filter_frag

			ENDCG
		}
	}
}
