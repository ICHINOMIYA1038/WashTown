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
            // テクスチャのピクセルデータを取得する
            Color32[] pixels = texture.GetPixels32();

            // すべてのピクセルを走査し、α値を反転させる
            for (int i = 0; i < pixels.Length; i++)
            {
                Color32 pixel = pixels[i];
                // α値が0より大きい場合は0に変更し、0の場合は255に変更する
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

            // ピクセルデータをテクスチャに書き込み、適用する
            texture.SetPixels32(pixels);
            texture.Apply();
        }
    }


