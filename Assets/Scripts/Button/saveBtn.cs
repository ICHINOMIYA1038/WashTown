using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveBtn : OriginalBtn
{
    [SerializeField] GameManager gameManager;
    [SerializeField] int option = 0;
    static readonly int EXITWITHSAVE = 0;
    static readonly int EXITWITHOUTSAVE = 1;
    static readonly int SAVEONLY = 2;
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
        if(option == EXITWITHSAVE)
        {
            save();
            exit();
        }
        else if(option == EXITWITHOUTSAVE)
        {
            exit();
        }
        else if(option == SAVEONLY)
        {
            save();
        }
    }

    void exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        # endif
    }

    void save()
    {
        gameManager.save();
    }
}
