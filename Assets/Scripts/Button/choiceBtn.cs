using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class choiceBtn : OriginalBtn
{
    public int index = 1;
    public choicedManager choicedManager;
    [SerializeField]
    protected GameObject panel;
    protected bool choiced = false;

    virtual public void Start()
    { 
        this.onClickCallback = clickEvent;
        panel.SetActive(false);
    }

    virtual public void clickEvent()
    {
        if (choiced) unchoiceEvent();
        else choiceEvent();
        choicedManager.choice(this);

    }

    virtual public void choiceEvent()
    {
        choiced = true;
        panel.SetActive(true);
    }

    virtual public void  unchoiceEvent()
    {
        choiced = false;
        panel.SetActive(false);
    }


}
