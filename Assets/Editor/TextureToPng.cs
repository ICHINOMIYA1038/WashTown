using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EditorWindowSample : EditorWindow
{
    public Object source;
    string fileName = "FileName";
    Texture texture;
    string status = "Idle";
    string recordButton = "Record";
    bool recording = false;
    float lastFrameTime = 0.0f;
    int capturedFrame = 0;
    [MenuItem("Editor/window")]
    private static void Create()
    {
        // 生成
        EditorWindowSample window = GetWindow<EditorWindowSample>("サンプル");
        // 最小サイズ設定
        window.minSize = new Vector2(320, 320);
        window.Show();
    }

    void OnGUI()
    {
        
        source = EditorGUILayout.ObjectField("ディレクトリを指定", source, typeof(Texture), true);
        texture = (Texture)source;
        GUI.DrawTexture(new Rect(0, 100, 320, 320), texture, ScaleMode.StretchToFill);
    }
}
