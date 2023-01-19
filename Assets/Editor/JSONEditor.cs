using UnityEngine;
using UnityEditor;

public class JsonEditorWindow : ScriptableWizard
{
    [SerializeField]
    private workJsonData m_jsonData = null;

    [MenuItem("Editor/JsonEditor")]
    public static void Open()
    {
        DisplayWizard<JsonEditorWindow>("JsonEditor", "Save");
    }

    /// <summary>
    /// ScriptableWizardのメインとなるボタンが押された際に呼ばれるよ！
    /// 今回の場合はDisplayWizardの第二引数で指定した"Save"ボタンが押されたとき。
    /// </summary>
    private void OnWizardCreate()
    {
        string json = JsonUtility.ToJson(m_jsonData);
        string path = EditorUtility.SaveFilePanel("名前を付けてJsonを保存しよう", "", "Setting", "json");

        System.IO.File.WriteAllText(path, json);

        // プロジェクトフォルダ内に保存された際の対応.
        AssetDatabase.Refresh();
    }

}
