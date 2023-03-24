Shader "Custom/s_TriTile_2"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Size ("Size", Float) = 0.5
        _Count ("Size", Int) = 0
        _MainTex_0 ("Albedo 0 (RGB)", 2D) = "white" {}
        _MainTex_1 ("Albedo 1 (RGB)", 2D) = "white" {}
        _MainTex_2 ("Albedo 2 (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex_0;
        sampler2D _MainTex_1;
        sampler2D _MainTex_2;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        half _Size;
        int _Count;
        fixed4 _Color;
        half sqrt3 = 1.7320508;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 aCol = tex2D (_MainTex_0, IN.uv_MainTex) * _Color;
            if (_Count == 0)
            {
                o.Albedo = aCol.rgb;
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = aCol.a;
                return;
            }else
            {
                fixed2 posA = fixed2(_Size/2, sqrt3 * _Size);
                fixed2 posB = fixed2(_Size/2, - sqrt3 * _Size);
                fixed2 posC = fixed2(-_Size, 0);
                
                fixed4 bCol = tex2D (_MainTex_1, IN.uv_MainTex) * _Color;
                fixed4 cCol = tex2D (_MainTex_2, IN.uv_MainTex) * _Color;
                half lengthA = length(_W posA);
                half lengthB = length(posB);
                half lengthC = length(posC);
                
                half4 col = aCol;
                col = lerp(col, bCol)
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            }

        }
        ENDCG
    }
    FallBack "Diffuse"
}
