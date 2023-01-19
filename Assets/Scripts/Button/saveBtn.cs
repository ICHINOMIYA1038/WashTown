using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveBtn : OriginalBtn
{
    [SerializeField] GameManager gameManager;
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
        gameManager.save();
    }
}
