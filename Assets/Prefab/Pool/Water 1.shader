

//水面が揺れるシェーダー
///参考;https://qiita.com/nonkapibara/items/8b328f8d3ffd4735a2f7
//

Shader "Unlit/Water"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Speed("Speed ",Range(0, 100)) = 2
        _SwingCycle("SwingCycle", Range(0.0,30.0)) = 9.0 // 揺れの周期
        _Amplitude("Amplitude", Range(0,10.0)) = 0.07 // 振り幅        
    }
        SubShader{
            //　水面を少し透明に設定する
            Tags { "RenderType" = "Transparent"  "Queue" = "Transparent"}
            Blend SrcAlpha OneMinusSrcAlpha
            Pass {
            CGPROGRAM
                        #pragma vertex vert
                        #pragma fragment frag
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
                        float _Speed;
                        float _SwingCycle;
                        float _Amplitude;
                        float4 _Color;

                        v2f vert(appdata v) {
                            v2f o;
                            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                            o.vertex = UnityObjectToClipPos(v.vertex);
                            
                            // スピードを設定する
                            float time = _Time.y * _Speed;
                            // 揺れ幅を設定
                            float offsetY = sin(time + v.vertex.x * _SwingCycle) + sin(time + v.vertex.z * _SwingCycle);
                            offsetY *= _Amplitude;
                            o.vertex.y += offsetY;
                            
                            return o;
                        }

                        fixed4 frag(v2f IN) : SV_Target{
                            fixed4 o = tex2D(_MainTex, IN.uv);
                        // 水面の透明度を0.7に設定する
                            o.a = 0.7f;
                            return o;
                        }

                    ENDCG
                        }

        }
        
}