﻿Shader "Hidden/PixelationShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Pixelation ("Pixelation", Range(1, 100)) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Pixelation;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 normalizedPixelation = _Pixelation * _ScreenParams.x / 1000.0;
                float2 pixUv = floor(i.uv * _ScreenParams / normalizedPixelation) / _ScreenParams * normalizedPixelation;

                fixed4 col = tex2D(_MainTex, pixUv);
                return col;
            }
            ENDCG
        }
    }
}
