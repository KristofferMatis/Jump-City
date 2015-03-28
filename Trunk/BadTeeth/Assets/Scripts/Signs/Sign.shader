Shader "Custom/Sign" 
{
	Properties 
	{
		_MainTexture ("MainTexture", 2D) = "white" {}
		_GrafitiTexture ("GrafitiTexture", 2D) = "white" {}
		_GrafitiGreyScale ("GrafitiGreyScale", 2D) = "white" {}
		
		_PercentPainted("PercentPainted", Range (0.0,1.0)) = 0.0
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }		
		Pass
		{			
			CGPROGRAM
			#pragma vertex vertShader
			#pragma fragment fragShader

			sampler2D _MainTexture;
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
				float4 main = tex2D (_MainTexture, IN.uv_MainTex);
				float4 grafiti = tex2D (_GrafitiTexture, IN.uv_MainTex);
				float4 greyScale =  tex2D (_GrafitiGreyScale, IN.uv_MainTex);
			
				float4 c = (greyScale.r > _PercentPainted && grafiti.a > 0.0f) ? grafiti: main;
				
				return c;
			}
			ENDCG
		} 
	}
	FallBack "Diffuse"
}
