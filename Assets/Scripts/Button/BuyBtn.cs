using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyBtn : OriginalBtn
{
    [SerializeField] PurchaseManager purchaseManager;
    void Start()
    {
        this.onClickCallback = clickEvent;
    }

    void clickEvent()
    {
        purchaseManager.buy();
    }
}
