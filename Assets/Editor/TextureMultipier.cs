using UnityEngine;
using UnityEditor;

public class TextureMultiplier : EditorWindow
{
    private Texture2D _texture1;
    private Texture2D _texture2;
    private string _outputPath = "Assets/OutputTexture.png";

    [MenuItem("Original/Texture Multiplier")]
    private static void ShowWindow()
    {
        var window = GetWindow<TextureMultiplier>();
        window.titleContent = new GUIContent("Texture Multiplier");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(20f);

        // テクスチャ1の入力欄
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Texture 1:");
        _texture1 = (Texture2D)EditorGUILayout.ObjectField(_texture1, typeof(Texture2D), false);
        EditorGUILayout.EndHorizontal();

        // テクスチャ2の入力欄
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Texture 2:");
        _texture2 = (Texture2D)EditorGUILayout.ObjectField(_texture2, typeof(Texture2D), false);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20f);

        // 出力先のパスの入力欄
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Output Path:");
        _outputPath = EditorGUILayout.TextField(_outputPath);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20f);

        // 実行ボタン
        if (GUILayout.Button("Multiply Textures"))
        {
            var textureImporter1 = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(_texture1)) as TextureImporter;
            textureImporter1.isReadable = true;
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(_texture1));

            var textureImporter2 = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(_texture2)) as TextureImporter;
            textureImporter2.isReadable = true;
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(_texture2));
            DebugLogTextureFormat(_texture1);
            DebugLogTextureFormat(_texture2);
            InvertAlpha(_texture1);
            MultiplyTextures();
        }
    }


    public void MultiplyTextures()
    {
        // テクスチャが2つ入力されているか確認する
        if (_texture1 == null || _texture2 == null)
        {
            Debug.LogError("Please input two textures.");
            return;
        }

        // テクスチャのサイズが同じか確認する
        if (_texture1.width != _texture2.width || _texture1.height != _texture2.height)
        {
            Debug.LogError("Texture sizes are different.");
            return;
        }

        // 出力するテクスチャを作成する
        var outputTexture = new Texture2D(_texture1.width, _texture1.height);

        // すべてのピクセルを走査し、テクスチャを乗算する
        for (int y = 0; y < outputTexture.height; y++)
        {
            for (int x = 0; x < outputTexture.width; x++)
            {
                // 2つのテクスチャのピクセルを取得する
                var color1 = _texture1.GetPixel(x, y);
                var color2 = _texture2.GetPixel(x, y);

                // 2つのピクセルの色を乗算して、出力する色を作成する
                var outputColor = new Color(color1.r * color2.r, color1.g * color2.g, color1.b * color2.b, color1.a * color2.a);
                outputTexture.SetPixel(x, y, outputColor);
            }
        }

        // ピクセルデータをテクスチャに書き込み、適用する
        outputTexture.Apply();

        // テクスチャをPNG形式にエンコードしてファイルに保存する
        var bytes = outputTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(_outputPath, bytes);

        // UnityのAssetDatabaseを更新する
        AssetDatabase.Refresh();
        Debug.Log("Output texture has been created.");
    }

    //特定のテクスチャのα値の反転
    public void InvertAlpha(Texture2D texture)
    {
        // テクスチャのピクセルデータを取得する
        Color[] pixels = texture.GetPixels();

        // すべてのピクセルを走査し、α値を反転させる
        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixel = pixels[i];
            // α値が0より大きい場合は0に変更し、0の場合は1.0fに変更する
            if (pixel.a > 0)
            {
                pixel.a = 0;
            }
            else
            {
                pixel.a = 1.0f;
            }
            pixels[i] = pixel;
        }

        // ピクセルデータをテクスチャに書き込み、適用する
        texture.SetPixels(pixels);
        texture.Apply();
    }

    //フォーマットエラーが起きたので、そのときはこっちを使う。
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

    public void DebugLogTextureFormat(Texture2D texture)
    {
        // テクスチャのフォーマットを取得する
        TextureFormat format = texture.format;

        // テクスチャのフォーマットを文字列に変換する
        string formatString = format.ToString();

        // デバッグログにテクスチャのフォーマットを出力する
        Debug.Log("Texture format: " + formatString);
    }


    public Texture2D ConvertDXT5(Texture2D srcTexture)
    {
        Texture2D texture = new Texture2D(srcTexture.width, srcTexture.height, TextureFormat.DXT5, false);
        texture.SetPixels(srcTexture.GetPixels());
        texture.Apply();
        return texture;
    }
}