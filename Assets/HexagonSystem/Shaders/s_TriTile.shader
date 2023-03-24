Shader "Unlit/s_TriTile"
{
    Properties
    {
        _Size ("Size", Float) = 0.5
        _Count ("Size", Int) = 0
        _MainTex_0 ("Texutre 0", 2D) = "white" {}
        _MainTex_1 ("Texutre 1", 2D) = "white" {}
        _MainTex_2 ("Texutre 2", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex_0;
            sampler2D _MainTex_1;
            sampler2D _MainTex_2;
            float4 _MainTex_0_ST;
            float4 _MainTex_1_ST;
            float4 _MainTex_2_ST;
            float _Size;
            int _Count;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex_0);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 aCol = tex2D(_MainTex_0, IN.uv_MainTex) * _Color;
                if (_Count == 0)
                {
                    return aCol;
                }
                else
                {
                    fixed2 posA = fixed2(_Size / 2, sqrt3 * _Size);
                    fixed2 posB = fixed2(_Size / 2, - sqrt3 * _Size);
                    fixed2 posC = fixed2(-_Size, 0);

                    fixed4 bCol = tex2D(_MainTex_1, IN.uv_MainTex) * _Color;
                    fixed4 cCol = tex2D(_MainTex_2, IN.uv_MainTex) * _Color;
                    half lengthA = length(_W posA);
                    half lengthB = length(posB);
                    half lengthC = length(posC);

                    half4 col = aCol;
                    
  
                }
                fixed4 col = tex2D(_MainTex_0, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}