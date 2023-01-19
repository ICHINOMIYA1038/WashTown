using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClose : OriginalBtn
{
    [SerializeField] GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        this.onClickCallback = closeCanvas;
    }


    void closeCanvas()
    {
        canvas.gameObject.SetActive(false);
    }
}
