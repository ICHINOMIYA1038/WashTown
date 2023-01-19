using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PurchaseBtn : OriginalBtn
{
    [SerializeField]
    int nextSceneIndex;
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
        SceneManager.LoadScene(nextSceneIndex);

    }
}
