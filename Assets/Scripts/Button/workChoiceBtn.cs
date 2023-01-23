using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class workChoiceBtn : OriginalBtn
{
    public int index = 1;
    public WorkPanelCon workPanelCon;
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
        workPanelCon.choice(this);

    }

    void choiceEvent()
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
