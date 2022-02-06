Shader "Hidden/Custom/ColorBlindCorrection"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

        half4 FragProtanopia(VaryingsDefault i) : SV_Target
        {
            half4 input = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            return half4(
                input.r * 0.567 + input.g * 0.433,
                input.r * 0.558 + input.g * 0.442,
                input.g * 0.242 + input.b * 0.758,
                input.a);
        }

        half4 FragDeuteranopia(VaryingsDefault i) : SV_Target
        {
            half4 input = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            half commonGreen = input.g * 0.3;            
            return half4(
                input.r * 0.625 + input.g * 0.375,
                input.r * 0.7 + commonGreen,
                commonGreen + input.b * 0.7,
                input.a);
        }

        half4 FragTritanopia(VaryingsDefault i) : SV_Target
        {
            half4 input = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            return half4(
                input.r * 0.95 + input.g * 0.05,
                input.g * 0.433 + input.b * 0.567,
                input.g * 0.475 + input.b * 0.525,
                input.a);
        }

        half4 FragAchromatopsia(VaryingsDefault i) : SV_Target
        {
            half4 input = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            float a = input.a;
            input = dot(input.rgb, half3(0.3, 0.59, 0.11));
            return half4(input.rgb, a);
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        
        Pass
        {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment FragProtanopia
            ENDHLSL
        }
        
        Pass
        {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment FragDeuteranopia
            ENDHLSL
        }

        Pass
        {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment FragTritanopia
            ENDHLSL
        }
        
        Pass
        {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment FragAchromatopsia
            ENDHLSL
        }
    }
}