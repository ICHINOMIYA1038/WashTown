using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class choicedManager : MonoBehaviour
{
    protected choiceBtn choicedBtn;
    protected int choicedIndex;

    // Start is called before the first frame update

    virtual public void choice(choiceBtn btn)
    {
        if (choicedBtn != null) choicedBtn.unchoiceEvent();
        if (btn == choicedBtn)
        {
            choicedBtn = null;
            choicedIndex = -1;
        }
        else
        {
            choicedBtn = btn;
            choicedIndex = choicedBtn.index;
        }
  
    }

    public virtual int getIndex()
    {
        if (choicedBtn == null)
        {
            return -1;
        }
        return choicedBtn.index;
    }
}
