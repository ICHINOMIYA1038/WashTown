using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class workStartBtn : OriginalBtn
{

    [SerializeField]
    WorkPanelCon workPanelCon;
    List<int> indexList;
    [SerializeField]
    ItemChoicedPanel itemChoicedPanel;

    public void Start()
    {
        this.onClickCallback = pushBtn;
    }

    public void pushBtn()
    {
        if (workPanelCon == null)
        {
            return;
        }
        var index = workPanelCon.getIndex();
        if (index == -1)
        {
            return;
        }
        itemChoicedPanel.extractIndex();
        indexList = itemChoicedPanel.indexList;
        GameManager.gameEnd = false;
        ItemManager.setItemList(indexList);
        SceneManager.LoadScene(index + 2);
       
    }

   
}

