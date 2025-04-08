Shader "Custom/StandardAlphaDiscard"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _MetallicGlossMap ("Metallic Map", 2D) = "black" {}
        _EmissionMap ("Emission Map", 2D) = "black" {}
        _EmissionColor ("Emission Color", Color) = (0,0,0,0)
        _Flip ("Flip X", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 300

        CGPROGRAM
        #pragma surface surf Standard alpha:fade fullforwardshadows

        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _MetallicGlossMap;
        sampler2D _EmissionMap;
        fixed4 _EmissionColor;
        half _Glossiness;
        half _Metallic;
        float _Flip;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float2 uv_MetallicGlossMap;
            float2 uv_EmissionMap;
        };

        float2 ApplyFlip(float2 uv, float flipX)
        {
            uv = uv * float2(flipX, 1.0) + float2(flipX < 0 ? 1 : 0, 0);
            return saturate(uv);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float flipX = 1.0;

            float2 uv = ApplyFlip(IN.uv_MainTex, flipX);
            fixed4 c = tex2D(_MainTex, uv);
            if (c.a < 0.01) discard;

            o.Albedo = c.rgb;
            o.Alpha = c.a;

            float2 uvBump = ApplyFlip(IN.uv_BumpMap, flipX);
            float2 uvMetal = ApplyFlip(IN.uv_MetallicGlossMap, flipX);
            float2 uvEmiss = ApplyFlip(IN.uv_EmissionMap, flipX);

            o.Normal = UnpackNormal(tex2D(_BumpMap, uvBump));
            o.Metallic = _Metallic * tex2D(_MetallicGlossMap, uvMetal).r;
            o.Smoothness = _Glossiness * tex2D(_MetallicGlossMap, uvMetal).a;
            o.Emission = tex2D(_EmissionMap, uvEmiss).rgb * _EmissionColor.rgb;
        }
        ENDCG
    }
}