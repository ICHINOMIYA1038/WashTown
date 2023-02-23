using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UVEditorWindow : EditorWindow
{
    [MenuItem("Original/UV Editor")]
    public static void ShowWindow()
    {
        GetWindow<UVEditorWindow>("UV Editor");
    }

    private void OnGUI()
    {
        GameObject selectedObject = Selection.activeGameObject;
        Mesh mesh = selectedObject.GetComponent<MeshFilter>().sharedMesh;
        Vector2[] uvs = mesh.uv;
        for (int i = 0; i < uvs.Length; i++)
        {
            Vector2 uv = uvs[i];
            Rect rect = GUILayoutUtility.GetRect(16, 16);
            EditorGUI.DrawRect(rect, Color.gray);
            EditorGUI.LabelField(rect, new GUIContent(string.Format("{0}: {1:F2}, {2:F2}", i, uv.x, uv.y)));
        }
    }
}