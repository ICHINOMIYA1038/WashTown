using UnityEditor;
using UnityEngine;
using System;
using System.IO;

/// <summary>
/// https://kyoro-s.com/unity-10/ からサンプルコードを利用,改変。
/// </summary>
public class ScreenShotWindow : EditorWindow
{
    private string _filePath = "Assets/ScreenShot/";

    //スクリーンショットボタンを表示する
    [MenuItem("Editor/ScreenShotWindow")]
    static void WindowOpen()
    {
        var window = EditorWindow.GetWindow<ScreenShotWindow>();
    }

    //エディタウィンドウの見た目を表示
    private void OnGUI()
    {
        ButtonDisp();
    }

    //撮影ボタンを表示する
    private void ButtonDisp()
    {
        if (GUILayout.Button("ScreenShot"))
        {
            ScreenShot();
        }
    }

    //スクリーンショットを行う
    private void ScreenShot()
    {
        string fileName = _filePath + DateTime.Now.ToString("yy-MM-dd-HH-mm-ss") + ".png";
        ScreenCapture.CaptureScreenshot(fileName);
        File.Exists(fileName);
        Debug.Log("118");
    }
}
