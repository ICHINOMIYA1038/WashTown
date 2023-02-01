using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBtn : OriginalBtn
{
    [SerializeField] GameObject canvas;
    bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        this.onClickCallback = clickEvent;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void clickEvent()
    {
        if (isOpen) close();
        else open();
    }

    public void close()
    {
        if (isOpen)
        {
            canvas.SetActive(false);
            isOpen = false;
       
        }
    }

    public void open()
    {
        if (!isOpen)
        {
            canvas.SetActive(true);
            isOpen = true;
        }

    }
}
