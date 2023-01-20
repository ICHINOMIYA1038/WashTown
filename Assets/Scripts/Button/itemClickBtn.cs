using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemClickBtn : OriginalBtn
{

    public PurchaseManager purchaseManager;
    public int itemIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        this.onClickCallback = clickEvent;
    }

    void clickEvent()
    {
        Debug.Log(purchaseManager.add(itemIndex, 1));
    }

    
}
