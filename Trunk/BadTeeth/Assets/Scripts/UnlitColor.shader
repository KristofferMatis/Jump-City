Shader "Custom/UnlitColor" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }		
		Pass
		{			
			CGPROGRAM
			#pragma vertex vertShader
			#pragma fragment fragShader

			float4 _Color;
			
			struct vertexInput 
			{
				float4 pos : POSITION;
			};
			
			struct fragmentInput 
			{
				float4 pos : POSITION;
			};

			fragmentInput vertShader (vertexInput IN) 
			{																   
			   	fragmentInput OUT;
			   	OUT.pos = mul(UNITY_MATRIX_MVP, IN.pos);
			   	return OUT;
			}
			
			float4 fragShader (fragmentInput IN) : COLOR
			{
				return _Color;
			}
			ENDCG
		} 
	}
	FallBack "Diffuse"
}
