//USING CHATGPT!!

Shader "Custom/BlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 10)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        half _BlurSize;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            float4 color = tex2D(_MainTex, IN.uv_MainTex);
            for (int i = 1; i <= 5; i++)
            {
                color += tex2D(_MainTex, IN.uv_MainTex + i * _BlurSize * float2(0.003, 0.003));
                color += tex2D(_MainTex, IN.uv_MainTex - i * _BlurSize * float2(0.003, 0.003));
            }
            color /= 11; // Averaging
            o.Albedo = color.rgb;
        }
        ENDCG
    }
}
