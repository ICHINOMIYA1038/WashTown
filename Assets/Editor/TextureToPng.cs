using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EditorWindowSample : EditorWindow
{
    public Object gameObject;
    public GameObject target;
    public WashableObject washableObject;
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
        gameObject = EditorGUILayout.ObjectField("ディレクトリを指定", gameObject, typeof(GameObject), true);
        target = (GameObject)gameObject;
        washableObject = target.GetComponent<WashableObject>();
        Texture2D src = washableObject.getDirtyTexture();
        
        source = EditorGUILayout.ObjectField("ディレクトリを指定", source, typeof(Texture2D), true);
        texture = (Texture2D)source;
        GUI.DrawTexture(new Rect(0, 100, 320, 320), src, ScaleMode.StretchToFill);
    }

    void getPixel(Texture2D src)
    {
        for(int i=0;  i<src.width; i++)
        {
            for (int j=0; j < src.height; j++)
            {

            }
        }
    }
}
