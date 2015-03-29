Shader "Sign menus" 
{
	Properties 
	{
		_GrafitiTexture ("GrafitiTexture", 2D) = "white" {}
		_GrafitiGreyScale ("GrafitiGreyScale", 2D) = "white" {}
		
		_PercentPainted("PercentPainted", Range (0.0,1.0)) = 0.0
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }		
		Pass
		{			
			Cull back
			ZWrite Off
			
			//Our blend equation is multiplicative
         	Blend SrcAlpha OneMinusSrcAlpha
		
			CGPROGRAM
			#pragma vertex vertShader
			#pragma fragment fragShader

			sampler2D _GrafitiTexture;
			sampler2D _GrafitiGreyScale;
			
			float _PercentPainted;
			
			struct vertexInput 
			{
				float4 pos : POSITION;
				float2 uv_MainTex : TEXCOORD0;
			};
			
			struct fragmentInput 
			{
				float4 pos : POSITION;
				float2 uv_MainTex : TEXCOORD0;
			};

			fragmentInput vertShader (vertexInput IN) 
			{																   
			   	fragmentInput OUT;
			   	OUT.uv_MainTex = IN.uv_MainTex;
			   	OUT.pos = mul(UNITY_MATRIX_MVP, IN.pos);
			   	return OUT;
			}
			
			float4 fragShader (fragmentInput IN) : COLOR
			{
				float4 grafiti = tex2D (_GrafitiTexture, IN.uv_MainTex);
				float4 greyScale =  tex2D (_GrafitiGreyScale, IN.uv_MainTex);
			
				float4 c = (greyScale.r <= _PercentPainted && grafiti.a > 0.0f) ? grafiti :(greyScale.r>_PercentPainted+0.1f)?  (greyScale.r>_PercentPainted+0.25f)?float4(0.0f,0.0f,0.0f,0.0f): grafiti * (_PercentPainted-0.2f): grafiti * _PercentPainted;
				return c;
			}
			ENDCG
		} 
	}
	FallBack "Diffuse"
}
