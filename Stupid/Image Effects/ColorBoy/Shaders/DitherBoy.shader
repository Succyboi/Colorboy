// Original shader from https://github.com/keijiro/KinoBinary

Shader "Hidden/DitherBoy"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _DitherTex("", 2D) = "" {}
        _Power("", Range(0, 1024)) = 1
    }

        CGINCLUDE

#include "UnityCG.cginc"

            sampler2D _MainTex;
        float2 _MainTex_TexelSize;

        sampler2D _DitherTex;
        float2 _DitherTex_TexelSize;

        half _Scale;
        half _Power;

        half4 frag(v2f_img i) : SV_Target
        {
            half4 source = tex2D(_MainTex, i.uv);

            // Dither pattern sample
            float2 dither_uv = i.uv * _DitherTex_TexelSize;
            dither_uv /= _MainTex_TexelSize * _Scale;
            half dither = tex2D(_DitherTex, dither_uv).a + 1 / 256;

            source += dither * (1.0 / _Power);

            return source;
        }

            ENDCG

            SubShader
        {
            Pass
            {
                ZTest Always Cull Off ZWrite Off
                CGPROGRAM
                #pragma multi_compile _ UNITY_COLORSPACE_GAMMA
                #pragma vertex vert_img
                #pragma fragment frag
                ENDCG
            }
        }
}
