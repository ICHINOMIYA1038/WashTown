using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャンバスマネージャー
/// 複数のキャンバスを取りまとめる。
/// 現在開いているキャンバスの情報を取得して、重複して開かれた場合には、以前、開かれていたキャンバスを非アクティブにする。
/// </summary>
public class CanvasManager : MonoBehaviour
{
    MenuBtn menuBtn;

    public void Start()
    {
        menuBtn = null;
    }
    // Start is called before the first frame update
    public void open(MenuBtn btn)
    {
        if (menuBtn != null)
        {
            menuBtn.close();
        }
        menuBtn = btn;
        
    }

    public void close()
    {
        menuBtn = null;
    }

    public bool isOpendAnyCanvas()
    {
        if (menuBtn == null) return false;
        else return true;

        // Update is called once per frame
    }
}
