using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
