Shader "GAF/GAFGlow" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BlurX ("Horizontal blur", Float) = 0
		_BlurY ("Vertical blur", Float) = 0
		_GlowColor ("Glow color", Color) = (1, 1, 1, 1)
		_Strength ("Strength", Float) = 1.0
	}

	SubShader
	{
		Tags 
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
		}
		
		Blend One Zero
		Cull Off
		Zwrite Off
		Lighting Off
		
		Pass 
		{
			CGPROGRAM
			
			#include "GAFFilters.cginc"

			#pragma vertex gaf_blur_vert
			#pragma fragment gaf_glow_frag

			ENDCG
		}
	}

	Fallback Off
}
