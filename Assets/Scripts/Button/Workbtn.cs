using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Workbtn : OriginalBtn
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
        canvas.SetActive(!isOpen);
        isOpen = !isOpen;
    }
}
