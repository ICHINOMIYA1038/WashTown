using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class choicedManager : MonoBehaviour
{
    protected choiceBtn choicedBtn;
    protected int choicedIndex;
    [SerializeField]
    TextMeshProUGUI indexText;
    // Start is called before the first frame update

    public void choice(choiceBtn btn)
    {
        if (choicedBtn != null) choicedBtn.unchoiceEvent();
        if (btn == choicedBtn)
        {
            choicedBtn = null;
            choicedIndex = 0;
        }
        else
        {
            choicedBtn = btn;
            choicedIndex = choicedBtn.index;
        }
        if (indexText != null)
        {
            indexText.text = "" + choicedIndex;
        }
    }

    public int getIndex()
    {
        if (choicedBtn == null)
        {
            return -1;
        }
        return choicedBtn.index;
    }
}
