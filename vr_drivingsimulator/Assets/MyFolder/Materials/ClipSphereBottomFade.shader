Shader "Custom/ClipSphereBottomFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ClipPlane ("Clip Plane Height", Float) = 0.0
        _FadeRange ("Fade Range", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        sampler2D _MainTex;
        float _ClipPlane;
        float _FadeRange;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            // フェードの計算
            float distance = IN.worldPos.y - _ClipPlane;
            float fade = 1.0;

            if (distance < 0)
            {
                fade = 0; // クリッププレーンより下の部分は完全に透明
            }
            else if (distance < _FadeRange)
            {
                fade = distance / _FadeRange; // フェード範囲内でのフェード計算
            }

            c.a *= fade;

            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Transparent/Cutout/VertexLit"
}
