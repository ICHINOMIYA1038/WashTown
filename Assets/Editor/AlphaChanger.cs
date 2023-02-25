using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AlphaChanger : EditorWindow
{
    private Texture2D _texture;

    [MenuItem("Original/Alpha Changer")]
    private static void OpenWindow()
    {
        var window = GetWindow<AlphaChanger>();
        window.titleContent = new GUIContent("Alpha Changer");
        window.Show();
    }

    private void OnGUI()
    {
        _texture = EditorGUILayout.ObjectField("Texture", _texture, typeof(Texture2D), false) as Texture2D;

        if (_texture != null)
        {
            if (_texture.format == TextureFormat.DXT5)
            {
                EditorGUILayout.HelpBox("This texture is  in DXT5 format.", MessageType.Warning);
                return;
            }

            if (GUILayout.Button("Invert Alpha"))
            {
                InvertAlpha32(_texture);
            }
        }
    }


        public void InvertAlpha32(Texture2D texture)
        {
            // �e�N�X�`���̃s�N�Z���f�[�^���擾����
            Color32[] pixels = texture.GetPixels32();

            // ���ׂẴs�N�Z���𑖍����A���l�𔽓]������
            for (int i = 0; i < pixels.Length; i++)
            {
                Color32 pixel = pixels[i];
                // ���l��0���傫���ꍇ��0�ɕύX���A0�̏ꍇ��255�ɕύX����
                if (pixel.a > 0)
                {
                    pixel.a = 0;
                }
                else
                {
                    pixel.a = 255;
                }
                pixels[i] = pixel;
            }

            // �s�N�Z���f�[�^���e�N�X�`���ɏ������݁A�K�p����
            texture.SetPixels32(pixels);
            texture.Apply();
        }
    }


