Shader "Unlit/DisplayShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DirtyTex ("Texture",2D) = "white" {}
        _Color("BlushColor", VECTOR) = (1,0,0,1)
        _TomatoColor("TomatoColor", VECTOR) = (1,1,0,1)
    }
    SubShader
    {
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _PaintUV;
            sampler2D _DirtyTex;
            float4 _Color;
            float4 _TomatoColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 dirt = tex2D(_DirtyTex, i.uv);
                if (dirt.a < 0.4f){
                    return col;
                }
                else if(dirt.a <= 0.6f){
                    return _TomatoColor;
                }

                else return _Color;
                
                


            }
            ENDCG
        }
    }
}
