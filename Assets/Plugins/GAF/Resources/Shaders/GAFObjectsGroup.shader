Shader "GAF/GAFObjectsGroup" 
{
	Properties 
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_StencilID ("Stencil ID", Float) = 0
		_CustomColorMultiplier ("Color multiplier", Color) = (1,1,1,1)
		_CustomColorOffset("Color offset", Vector) = (0,0,0,0)
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
		Lighting On
		
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
			
			#include "GAFShaderBase.cginc"

			#pragma multi_compile GAF_VERTICES_TRANSFORM_ON GAF_VERTICES_TRANSFORM_OFF
			#pragma glsl_no_auto_normalization

			#pragma vertex gaf_base_vert_group
			#pragma fragment gaf_base_frag

			ENDCG
		}
	}
}
