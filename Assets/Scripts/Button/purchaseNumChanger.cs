using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class purchaseNumChanger : OriginalBtn
{
    [SerializeField] PurchaseManager purchaseManager;
    [SerializeField] int flag;
    [SerializeField] int index;
    void Start()
    {
        this.onClickCallback = clickEvent;
    }

    void clickEvent()
    {
        if(flag == 0)
        {
            purchaseManager.deleteNum(index);
        }
        else
        {
            purchaseManager.addNum(index);
        }
        

    }
}
