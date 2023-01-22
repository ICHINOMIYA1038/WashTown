using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClose : OriginalBtn
{
    [SerializeField] GameObject canvas;
    [SerializeField] CanvasManager canvasManager;
    // Start is called before the first frame update
    void Start()
    {
        this.onClickCallback = closeCanvas;
    }


    void closeCanvas()
    {
        canvas.gameObject.SetActive(false);
        canvasManager.close();
    }
}
