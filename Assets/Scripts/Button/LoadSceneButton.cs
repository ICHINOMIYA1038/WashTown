using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : OriginalBtn
{
    [SerializeField]int index = 1;
    
    void Start()
    {
       // Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = true;
        this.onClickCallback = clickEvent;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void clickEvent()
    {
        SceneManager.LoadScene(index);

    }
}
