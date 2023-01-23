using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class workStartBtn : OriginalBtn
{

    [SerializeField]
    WorkPanelCon workPanelCon;

    private void Start()
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
        if (index == 0)
        {
            return;
        }
        SceneManager.LoadScene(index);
    }
}
