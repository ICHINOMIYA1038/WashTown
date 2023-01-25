using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class choiceBtn : OriginalBtn
{
    public int index = 1;
    public choicedManager choicedManager;
    [SerializeField]
    GameObject panel;
    bool choiced = false;

    void Start()
    { 
        this.onClickCallback = clickEvent;
        panel.SetActive(false);
    }

    void clickEvent()
    {
        if (choiced) unchoiceEvent();
        else choiceEvent();
        choicedManager.choice(this);

    }

    public void choiceEvent()
    {
        choiced = true;
        panel.SetActive(true);
    }

    public void unchoiceEvent()
    {
        choiced = false;
        panel.SetActive(false);
    }
}
