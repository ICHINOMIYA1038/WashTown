Shader "Custom/Cloud" {
        Properties{
            _MainTex("Base (RGB)", 2D) = "white" {}
            _CloudColor("Cloud Color", Color) = (1,1,1,1)
            _Speed("Speed", Range(0, 10)) = 1
        }

            SubShader{
                Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
                LOD 100

                Pass {
                    CGPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag
                    #pragma multi_compile_fog

                    #include "UnityCG.cginc"

                    struct appdata {
                        float4 vertex : POSITION;
                        float2 uv : TEXCOORD0;
                    };

                    struct v2f {
                        float2 uv : TEXCOORD0;
                        float4 vertex : SV_POSITION;
                    };

                    sampler2D _MainTex;
                    float4 _MainTex_ST;
                    float4 _CloudColor;
                    float _Speed;

                    v2f vert(appdata v) {
                        v2f o;
                        o.vertex = UnityObjectToClipPos(v.vertex);
                        o.uv = v.uv;
                        return o;
                    }

                    float4 frag(v2f i) : SV_Target {
                        float2 uv = i.uv;
                        float4 col = tex2D(_MainTex, uv);
                        float4 cloud = _CloudColor * col.r;
                        cloud.a *= col.a;
                        float noise = (tex2D(_MainTex, i.uv + _Time.y * _Speed).r - 0.5) * 2.0;
                        cloud.rgb += noise * 0.5;
                        return cloud;
                    }
                    ENDCG
                }
            }
                FallBack "Diffuse"
    }