using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;


public class AlphaEditorWindow : EditorWindow
{
    private Texture2D _texture;

    [MenuItem("Original/Alpha Editor Window")]
    private static void OpenWindow()
    {
        var window = GetWindow<AlphaEditorWindow>();
        window.titleContent = new GUIContent("Alpha Editor");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a texture:");

        // テクスチャを選択する
        _texture = (Texture2D)EditorGUILayout.ObjectField(_texture, typeof(Texture2D), false);

        // テクスチャが選択されている場合
        if (_texture != null)
        {
            // テクスチャのサイズを取得する
            var textureSize = new Vector2(_texture.width, _texture.height);

            // テクスチャを描画する
            GUILayout.Box(_texture, GUILayout.Width(textureSize.x), GUILayout.Height(textureSize.y));

            // α値を変更するボタンを表示する
            if (GUILayout.Button("Invert Alpha"))
            {
                InvertAlpha(_texture);
            }
        }
    }

    private void InvertAlpha(Texture2D texture)
    {
        // テクスチャのピクセルデータを取得する
        Color32[] pixels = texture.GetPixels32();

        // すべてのピクセルを走査し、α値を反転させる
        for (int i = 0; i < pixels.Length; i++)
        {
            Color32 pixel = pixels[i];
            // α値が0の場合は1に、0以外の場合は0に変更する
            pixel.a = (pixel.a == 0) ? (byte)1 : (byte)0;
            pixels[i] = pixel;
        }

        // ピクセルデータをテクスチャに書き込み、適用する
        texture.SetPixels32(pixels);
        texture.Apply();
    }
}
