using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuBtn : OriginalBtn
{
    [SerializeField] CanvasManager canvasManager;
    [SerializeField] GameObject canvas;
    [SerializeField] GameManager gameManager;

    bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        this.onClickCallback = clickEvent;
        

    }

    public void InvokeMyEvent()
    {
        if (gameManager == null)
        {
            return;
        }
        WorkPanelCon choicedmanager = gameManager.GetComponent<WorkPanelCon>();
        
        choicedmanager.openEvent();
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
            canvasManager.close();
        }
    }

    public void open()
    {
        if (!isOpen)
        {
            InvokeMyEvent();
            canvas.SetActive(true);
            isOpen = true;
            canvasManager.open(this);
        }

    }
}
