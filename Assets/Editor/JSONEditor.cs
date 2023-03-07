using UnityEngine;
using UnityEditor;
using System.IO;

public class JsonEditor : EditorWindow
{
    private string[] jsonFilePaths = new string[3]; // 3つのJSONファイルパスを格納する文字列の配列
    private int flag = 0; // JSONファイルを切り替える整数のフラグ
    private object jsonData; // JSONデータを格納するオブジェクト

    [MenuItem("Original/Json Editor")]
    static void Init()
    {
        JsonEditor window = (JsonEditor)EditorWindow.GetWindow(typeof(JsonEditor));
        window.Show();
    }

    void OnGUI()
    {
        // JSONファイルを選択するためのファイルダイアログを表示
        GUILayout.Label("Select JSON Files:");
        for (int i = 0; i < 3; i++)
        {
            GUILayout.BeginHorizontal();
            jsonFilePaths[i] = GUILayout.TextField(jsonFilePaths[i]);
            if (GUILayout.Button("...", GUILayout.Width(30)))
            {
                jsonFilePaths[i] = EditorUtility.OpenFilePanel("Select JSON File", "", "json");
            }
            GUILayout.EndHorizontal();
        }

        // JSONファイルを切り替えるフラグを表示
        flag = EditorGUILayout.IntSlider("Flag", flag, 0, 2);

        // JSONファイルを表示する部分
        if (jsonData != null)
        {
            GUILayout.Label("JSON Data:");
            GUILayout.TextArea(JsonUtility.ToJson(jsonData));
        }

        // JSONファイルを編集する部分
        if (jsonData != null)
        {
            GUILayout.Label("Edit JSON Data:");
            jsonData = DrawJsonDataEditor(jsonData);
        }

        // セーブボタン
        if (GUILayout.Button("Save"))
        {
            if (jsonData != null)
            {
                string json = JsonUtility.ToJson(jsonData);
                File.WriteAllText(jsonFilePaths[flag], json);
            }
        }

        // ロードボタン
        if (GUILayout.Button("Load"))
        {
            if (!string.IsNullOrEmpty(jsonFilePaths[flag]))
            {
                string json = File.ReadAllText(jsonFilePaths[flag]);
                jsonData = JsonUtility.FromJson(json, typeof(object));
                Debug.Log(jsonData);
            }
        }
    }

    private object DrawJsonDataEditor<T>(T data)
    {
        object result = null;

        if (data == null)
        {
            return result;
        }

        // 型に応じたGUIコントロールを作成する
        if (data is int)
        {
            result = EditorGUILayout.IntField((int)(object)data);
        }
        else if (data is float)
        {
            result = EditorGUILayout.FloatField((float)(object)data);
        }
        else if (data is string)
        {
            result = EditorGUILayout.TextField((string)(object)data);
        }
        else if (data is bool)
        {
            result = EditorGUILayout.Toggle((bool)(object)data);
        }
        else if (data.GetType().IsArray)
        {
            System.Collections.IList list = (System.Collections.IList)(object)data;
            int count = EditorGUILayout.IntField("Size", list.Count);
            if (count != list.Count)
            {
                while (count > list.Count)
                {
                    list.Add(null);
                }
                while (count < list.Count)
                {
                    list.RemoveAt(list.Count - 1);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = DrawJsonDataEditor(list[i]);
            }
            result = list;
        }
        else if (data is System.Collections.IDictionary)
        {
            System.Collections.IDictionary dict = (System.Collections.IDictionary)(object)data;
            foreach (object key in dict.Keys)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(key.ToString(), GUILayout.Width(100));
                dict[key] = DrawJsonDataEditor(dict[key]);
                EditorGUILayout.EndHorizontal();
            }
            result = dict;
        }

        return result;
    }
}