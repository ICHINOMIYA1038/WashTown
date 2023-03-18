

//���ʂ��h���V�F�[�_�[
///�Q�l;https://qiita.com/nonkapibara/items/8b328f8d3ffd4735a2f7
//

Shader "Unlit/Water"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Speed("Speed ",Range(0, 100)) = 2
        _SwingCycle("SwingCycle", Range(0.0,30.0)) = 9.0 // �h��̎���
        _Amplitude("Amplitude", Range(0,10.0)) = 0.07 // �U�蕝        
    }
        SubShader{
            //�@���ʂ����������ɐݒ肷��
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
                            
                            // �X�s�[�h��ݒ肷��
                            float time = _Time.y * _Speed;
                            // �h�ꕝ��ݒ�
                            float offsetY = sin(time + v.vertex.x * _SwingCycle) + sin(time + v.vertex.z * _SwingCycle);
                            offsetY *= _Amplitude;
                            o.vertex.y += offsetY;
                            
                            return o;
                        }

                        fixed4 frag(v2f IN) : SV_Target{
                            fixed4 o = tex2D(_MainTex, IN.uv);
                        // ���ʂ̓����x��0.7�ɐݒ肷��
                            o.a = 0.7f;
                            return o;
                        }

                    ENDCG
                        }

        }
        
}