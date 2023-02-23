using UnityEngine;
using UnityEditor;

// カスタムエディタのクラス
[CustomEditor(typeof(Texture2D))]
public class Texture2DEditor : Editor
{
    private Texture2D texture;  // 表示するテクスチャ
    private Color color = Color.white; // テクスチャに適用する色

    // エディターのGUIを表示する
    public override void OnInspectorGUI()
    {
        // テクスチャの情報を取得する
        texture = target as Texture2D;

        // テクスチャがnullの場合は何もしない
        if (texture == null)
        {
            return;
        }

        // テクスチャのプレビューを表示する
        GUILayout.Label(texture, GUILayout.MaxHeight(200));

        // テクスチャに適用する色を設定する
        color = EditorGUILayout.ColorField("Color", color);

        // テクスチャのピクセルデータを取得する
        Color[] pixels = texture.GetPixels();

        // ピクセルデータを変更する
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] *= color;
        }

        // 変更したピクセルデータをテクスチャに適用する
        texture.SetPixels(pixels);
        texture.Apply();
    }
}