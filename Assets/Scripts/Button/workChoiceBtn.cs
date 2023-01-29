using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class workChoiceBtn : choiceBtn
{   
    [SerializeField] public GameObject comingSoonPanel;
    /// <summary>
    /// status 0:�������@1:����@-1:�s��
    /// </summary>
    public int status;

    override public void Start()
    {
        this.onClickCallback = clickEvent;
        panel.SetActive(false);
    }

    override public void clickEvent()
    {
        if (choiced) unchoiceEvent();
        else choiceEvent();
        choicedManager.choice(this);

    }

    override public void choiceEvent()
    {
        choiced = true;
        panel.SetActive(true);
    }

    override public void unchoiceEvent()
    {
        choiced = false;
        panel.SetActive(false);
    }

    public int getStatus()
    {
        return status;
    }

}
